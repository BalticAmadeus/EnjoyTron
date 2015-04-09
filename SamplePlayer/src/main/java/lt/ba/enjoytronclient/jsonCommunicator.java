/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package lt.ba.enjoytronclient;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.Formatter;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;

import org.json.simple.parser.*;
import org.json.simple.*;

/**
 *
 * @author Titas Lapinskas <t.lapinskas@ba.lt>
 */
public class jsonCommunicator implements Runnable {

    private static String postToURL(String url, String message, DefaultHttpClient httpClient) throws IOException,
                                                                                                     IllegalStateException,
                                                                                                     UnsupportedEncodingException,
                                                                                                     RuntimeException {
        HttpPost postRequest = new HttpPost(url);

        StringEntity input = new StringEntity(message);
        input.setContentType("application/json");
        postRequest.setEntity(input);

        HttpResponse response = httpClient.execute(postRequest);

        if (response.getStatusLine().getStatusCode() != 200) {
            throw new RuntimeException("Failed : HTTP error code : " +
                    response.getStatusLine().getStatusCode());
        }

        BufferedReader br = new BufferedReader(
                new InputStreamReader((response.getEntity().getContent())));

        String output;
        StringBuilder totalOutput = new StringBuilder();
        while ((output = br.readLine()) != null) {
            totalOutput.append(output);
        }
        postRequest.completed();
        return totalOutput.toString();
    }

    static String calculateHash(String teamName, String userName, String sessionID, String sequence, String pass) {
        String temp = teamName + ":" + userName + ":" + sessionID + ":" + sequence + pass;

        MessageDigest digest;
        String digestStr;
        try {
            digest = MessageDigest.getInstance("SHA-1");
            digest.update(temp.getBytes("utf8"));
            byte[] digestBytes = digest.digest();
            digestStr = javax.xml.bind.DatatypeConverter.printHexBinary(digestBytes);
        }
        catch (NoSuchAlgorithmException ex) {
            System.err.println("No such alorithm");
            return "";
        }
        catch (UnsupportedEncodingException ex) {
            System.out.println("Unsupported encoding");
            return "";
        }
        return digestStr.toLowerCase();
    }

    static String calculateChallengeResponse(String challenge, String pass) {
        String temp = challenge + pass;

        MessageDigest digest;
        String digestStr;
        try {
            digest = MessageDigest.getInstance("SHA-1");
            digest.update(temp.getBytes("utf8"));
            byte[] digestBytes = digest.digest();
            digestStr = javax.xml.bind.DatatypeConverter.printHexBinary(digestBytes);
        }
        catch (NoSuchAlgorithmException ex) {
            System.err.println("No such alorithm");
            return "";
        }
        catch (UnsupportedEncodingException ex) {
            System.out.println("Unsupported encoding");
            return "";
        }
        return digestStr.toLowerCase();
    }

    private DefaultHttpClient httpClient = new DefaultHttpClient();
    private String sessionId = "";
    private String userName = "";
    private int sequenceNumber = 0;
    private String playerId = "";
    private String gameId = "";
    public String clientName = "";

    final String initLoginMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"0\",\"SessionId\":\"0\",\"TeamName\":\"%3$s\"}}";
    final String completeLoginMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"0\",\"SessionId\":\"0\",\"TeamName\":\"%3$s\"}, \"ChallengeResponse\":\"%4$s\" }";
    final String CreatePlayerMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"%3$s\",\"SessionId\":\"%4$s\",\"TeamName\":\"%5$s\"}}";
    final String WaitGameMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"%3$s\",\"SessionId\":\"%4$s\",\"TeamName\":\"%5$s\"}, \"PlayerId\":\"%6$s\"}";
    final String PlayerViewMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"%3$s\",\"SessionId\":\"%4$s\",\"TeamName\":\"%5$s\"}, \"PlayerId\":\"%6$s\"}";
    final String RoundResultsMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"%3$s\",\"SessionId\":\"%4$s\",\"TeamName\":\"%5$s\"}, \"PlayerId\":\"%6$s\"}";
    final String WaitRoundMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"%3$s\",\"SessionId\":\"%4$s\",\"TeamName\":\"%5$s\"}, \"PlayerId\":\"%6$s\", \"RefTurn\":\"%7$s\"}";
    final String PerformMoveMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"%3$s\",\"SessionId\":\"%4$s\",\"TeamName\":\"%5$s\"}, \"PlayerId\":\"%6$s\",\"Position\":{\"Col\":\"%7$s\", \"Row\":\"%8$s\"}}";
    final String LeaveGameMsg = "{\"Auth\":{\"AuthCode\":\"%1$s\", \"ClientName\":\"%2$s\",\"SequenceNumber\":\"%3$s\",\"SessionId\":\"%4$s\",\"TeamName\":\"%5$s\"}, \"PlayerId\":\"%6$s\"}";

    public boolean createPlayer() {

        String urlCreatePlayer = baseURL + "/json/CreatePlayer";

        Formatter formatter = new Formatter();
        sequenceNumber++;
        String Hash = calculateHash("Auth", clientName, sessionId, Integer.toString(sequenceNumber), "Secret123#");
        String message = formatter.format(CreatePlayerMsg, Hash, clientName, sequenceNumber, sessionId, "Auth").toString();

        if (logIt)
            System.out.println(clientName + " Attempting to call createPlayer with message: \n" + message);
        String response = "";
        try {
            response = postToURL(urlCreatePlayer, message, httpClient);
            if (logIt)
                System.out.println(clientName + " Got this from Create Player: " + response);

            JSONParser json = new JSONParser();
            JSONObject objComp = (JSONObject) json.parse(response);
            String status = objComp.get("Status").toString();
            if (!"OK".equals(status)) {
                return false;
            }

            playerId = objComp.get("PlayerId").toString();
            if (playerId == null || playerId.equals(""))
                return false;
        }
        catch (IOException ex) {
            System.err.println("IO exception: " + ex.getMessage());
        }
        catch (RuntimeException ex) {
            System.err.println("RunTime exception: " + ex.getMessage());
        }
        catch (ParseException ex) {
            System.err.println("Parse exception: " + ex.getMessage());
        }
        return true;
    }

    public boolean waitForGame() {

        while (true) {

            String urlWaitGameStart = baseURL + "/json/WaitGameStart";

            Formatter formatter = new Formatter();
            sequenceNumber++;
            String Hash = calculateHash("Auth", clientName, sessionId, Integer.toString(sequenceNumber), "Secret123#");
            String message = formatter.format(WaitGameMsg, Hash, clientName, sequenceNumber, sessionId, "Auth", playerId).toString();

            if (logIt)
                System.out.println(clientName + " Attempting to call WaitGameStart with message: \n" + message);
            String response = "";
            try {
                response = postToURL(urlWaitGameStart, message, httpClient);
                if (logIt)
                    System.out.println(clientName + " Got this from Wait Game Start : " + response);

                JSONParser json = new JSONParser();
                JSONObject objComp = (JSONObject) json.parse(response);
                String status = objComp.get("Status").toString();
                if (!"OK".equals(status)) {
                    return false;
                }

                // Got status OK - need to see if got gameId
                Long game = (Long) objComp.get("GameId");
                if (game != -1) {
                    gameId = game.toString();
                    return true;
                }

            }
            catch (IOException ex) {
                System.err.println("IO exception: " + ex.getMessage());
            }
            catch (RuntimeException ex) {
                System.err.println("RunTime exception: " + ex.getMessage());
                // Most probably it is just null, need to continue
            }
            catch (ParseException ex) {
                System.err.println("Parse exception: " + ex.getMessage());
            }
        }
    }

    private long rows = 0;
    private long columns = 0;
    ArrayList map;
    private long currentCol = 0;
    private long currentRow = 0;
    private String gameStatus = "";
    private long roundId = 0;

    public boolean getPlayerView() {

        String urlPlayerView = baseURL + "/json/GetPlayerView";

        Formatter formatter = new Formatter();
        sequenceNumber++;
        String Hash = calculateHash("Auth", clientName, sessionId, Integer.toString(sequenceNumber), "Secret123#");
        String message = formatter.format(PlayerViewMsg, Hash, clientName, sequenceNumber, sessionId, "Auth", playerId).toString();

        if (logIt)
            System.out.println(clientName + " Attempting to call getPlayerView with message: \n" + message);
        String response = "";
        roundId = 0;
        try {
            response = postToURL(urlPlayerView, message, httpClient);
            if (logIt)
                System.out.println(clientName + " Got this from Get Player View: " + response);

            JSONParser json = new JSONParser();
            JSONObject objComp = (JSONObject) json.parse(response);
            String status = objComp.get("Status").toString();
            gameStatus = status;
            if (!"OK".equals(status)) {
                return false;
            }

            // Parse map
            rows = (Long) ((Map) objComp.get("Map")).get("Height");
            columns = (Long) ((Map) objComp.get("Map")).get("Width");

            map = (ArrayList) ((Map) objComp.get("Map")).get("Rows");

            long index = (Long) objComp.get("Index");
            ArrayList playerStates = (ArrayList) objComp.get("PlayerStates");
            Map onePlayer = (Map) playerStates.get((int) index);
            gameStatus = (String) onePlayer.get("Condition");
            Map coords = (Map) onePlayer.get("Position");

            currentRow = (Long) coords.get("Row");
            currentCol = (Long) coords.get("Col");
            roundId = (long) objComp.get("Turn");

            // Outputing
            if (logIt)
                System.out.println(clientName + " Got rows: " + rows + " columns: " + columns + " Current round is: " + roundId);
            if (logIt)
                System.out.println(
                        clientName + " My index was: " + index + " So I am at " + currentRow + ":" + currentCol + " and my status: " + gameStatus);
            for (int i = 0; i < map.size(); i++) {
                if (logIt)
                    System.out.println(clientName + " " + map.get(i));
            }

        }
        catch (IOException ex) {
            System.err.println("IO exception: " + ex.getMessage());
        }
        catch (RuntimeException ex) {
            System.err.println("RunTime exception: " + ex.getMessage());
        }
        catch (ParseException ex) {
            System.err.println("Parse exception: " + ex.getMessage());
        }
        return true;
    }

    public boolean LeaveGame() {

        String urlLeaveGame = baseURL + "/json/LeaveGame";

        Formatter formatter = new Formatter();
        sequenceNumber++;
        String Hash = calculateHash("Auth", clientName, sessionId, Integer.toString(sequenceNumber), "Secret123#");
        String message = formatter.format(LeaveGameMsg, Hash, clientName, sequenceNumber, sessionId, "Auth", playerId).toString();

        if (logIt)
            System.out.println(clientName + " Attempting to call Leave Game with message: \n" + message);
        String response = "";
        try {
            response = postToURL(urlLeaveGame, message, httpClient);
            if (logIt)
                System.out.println(clientName + " Got this from Leave Game: " + response);

            JSONParser json = new JSONParser();
            JSONObject objComp = (JSONObject) json.parse(response);
            String status = objComp.get("Status").toString();
            gameStatus = status;
            if (!"OK".equals(status)) {
                return false;
            }
        }
        catch (IOException ex) {
            System.err.println("IO exception: " + ex.getMessage());
        }
        catch (RuntimeException ex) {
            System.err.println("RunTime exception: " + ex.getMessage());
        }
        catch (ParseException ex) {
            System.err.println("Parse exception: " + ex.getMessage());
        }
        return true;
    }

    public boolean waitForRound() {

        while (true) {

            String urlWaitRound = baseURL + "/json/WaitNextTurn";

            Formatter formatter = new Formatter();
            sequenceNumber++;
            String Hash = calculateHash("Auth", clientName, sessionId, Integer.toString(sequenceNumber), "Secret123#");
            String message = formatter.format(WaitRoundMsg, Hash, clientName, sequenceNumber, sessionId, "Auth", playerId,
                                              Long.toString(roundId)).toString();

            if (logIt)
                System.out.println(clientName + " Attempting to call WaitNextRound with message: \n" + message);
            String response = "";
            try {
                response = postToURL(urlWaitRound, message, httpClient);
                if (logIt)
                    System.out.println(clientName + " Got this from Wait Next Round : " + response);

                JSONParser json = new JSONParser();
                JSONObject objComp = (JSONObject) json.parse(response);
                String status = objComp.get("Status").toString();
                if (!"OK".equals(status)) {
                    return false;
                }

                Boolean finished = (Boolean) objComp.get("GameFinished");
                if (finished) {
                    // Need to extract new status
                    gameStatus = (String) objComp.get("FinishCondition");
                    return true;
                }

                // Got status OK - need to see if got gameId
                Boolean turn = (Boolean) objComp.get("TurnComplete");
                if (turn) {
                    return true;
                }

            }
            catch (IOException ex) {
                System.err.println("IO exception: " + ex.getMessage());
            }
            catch (RuntimeException ex) {
                System.err.println("RunTime exception: " + ex.getMessage());
                // Most probably it is just null, need to continue
            }
            catch (ParseException ex) {
                System.err.println("Parse exception: " + ex.getMessage());
            }
        }
    }

    public boolean MakeMove() {

        String urlMakeMove = baseURL + "/json/PerformMove";

        Formatter formatter = new Formatter();
        sequenceNumber++;

        // CalculatePosition:
        int newRow = (int) currentRow;
        int newCol = (int) currentCol;
        // Priority Top, the Down, then Left, the Right
        if (newRow - 1 >= 0 && ((String) map.get(newRow - 1)).charAt(newCol) == '.') {
            newRow--;
        }
        else if (newRow + 1 <= rows && ((String) map.get(newRow + 1)).charAt(newCol) == '.') {
            newRow++;
        }
        else if (newCol - 1 >= 0 && ((String) map.get(newRow)).charAt(newCol - 1) == '.') {
            newCol--;
        }
        else if (newCol + 1 <= columns && ((String) map.get(newRow)).charAt(newCol + 1) == '.') {
            newCol++;
        }

        if (logIt)
            System.out.println(
                    clientName + " Coords were: " + currentRow + ":" + currentCol + " Making into: " + newRow + ":" + newCol);

        String Hash = calculateHash("Auth", clientName, sessionId, Integer.toString(sequenceNumber), "Secret123#");
        String message = formatter.format(PerformMoveMsg, Hash, clientName, sequenceNumber, sessionId, "Auth",
                                          playerId, Integer.toString(newCol), Integer.toString(newRow)).toString();

        if (logIt)
            System.out.println(clientName + " Attempting to call PerformMove with message: \n" + message);
        String response = "";
        try {
            response = postToURL(urlMakeMove, message, httpClient);
            if (logIt)
                System.out.println(clientName + " Got this from PerformMove : " + response);

            JSONParser json = new JSONParser();
            JSONObject objComp = (JSONObject) json.parse(response);
            String status = objComp.get("Status").toString();
            if (!"OK".equals(status)) {
                return false;
            }

            // Got status OK - need to see if got gameId
//                Long game = (Long)objComp.get("GameId");
//                if (game != -1 )
//                {
//                    gameId = game.toString();
//                    return true;
//                }
        }
        catch (IOException ex) {
            System.err.println("IO exception: " + ex.getMessage());
        }
        catch (RuntimeException ex) {
            System.err.println("RunTime exception: " + ex.getMessage());
            // Most probably it is just null, need to continue
        }
        catch (ParseException ex) {
            System.err.println("Parse exception: " + ex.getMessage());
        }
        return true;
    }

// NoAuth
// Auth  password: Secret123#
    public void run() {
        String urlInit = baseURL + "/json/InitLogin";
        String urlCompl = baseURL + "/json/CompleteLogin";
        String Hash = calculateHash("Auth", clientName, "0", "0", "Secret123#");
        //String message = "{\"Auth\":{\"AuthCode\":\"" + Hash + "\", \"ClientName\":\"Client\",\"SequenceNumber\":\"0\",\"SessionId\":\"0\",\"TeamName\":\"NoAuth\"}}";
        Formatter formatter = new Formatter();
        String message = formatter.format(initLoginMsg, Hash, clientName, "Auth").toString();
        formatter.close();

        if (logIt)
            System.out.println(clientName + " Attempting to call InitLogin with message: \n" + message);
        try {

            String response;
            JSONParser json = new JSONParser();
            response = postToURL(urlInit, message, httpClient);
            if (logIt)
                System.out.println(clientName + " Got this: " + response);

            JSONObject obj = (JSONObject) json.parse(response);
            String status = obj.get("Status").toString();

            if ("OK".equals(status)) {
                // Can continue with login
                String challenge = obj.get("Challenge").toString();
                if (logIt)
                    System.out.println(clientName + " Can continue with login, my challenge key is: " + challenge);

                // Pass is empty here, Hash is still the same
                String respHash = calculateChallengeResponse(calculateChallengeResponse(challenge, "Secret123#"), "Secret123#");
                Formatter compForm = new Formatter();
                String completeMessage = compForm.format(completeLoginMsg, Hash, clientName, "Auth", respHash).toString();

                if (logIt)
                    System.out.println(clientName + " Going to invoke completeLogin method with message: " + completeMessage);
                response = postToURL(urlCompl, completeMessage, httpClient);

                if (logIt)
                    System.out.println(clientName + " CompleteLogin responsed with: " + response);

                JSONObject objComp = (JSONObject) json.parse(response);
                String StatusComp = objComp.get("Status").toString();
                if ("OK".equals(StatusComp)) {
                    sessionId = objComp.get("SessionId").toString();
                    System.out.println(clientName + " Finished login. My session ID is: " + sessionId);

                    if (createPlayer()) {
                        if (logIt)
                            System.out.println(clientName + " Player successfully created.");
                    }
                    else {
                        if (logIt)
                            System.err.println(clientName + " Couldn't create player - leaving");
                        return;
                    }
                }
                else {
                    System.err.println(clientName + " Couldn't complete login - leaving");
                    return;
                }
            }
            else {
                System.err.println(clientName + " Couldn't init login - leaving");
                return;
            }

            // At this point I have session ID, correct sequence and player id.
            // Now need to wait for game to start
            // Forever loop!!!
            while (true) {
                rows = 0;
                columns = 0;
                map = null;
                currentCol = 0;
                currentRow = 0;
                gameStatus = "";
                roundId = 0;

                if (!waitForGame()) {
                    System.err.println(clientName + " Something bad happened  - server crashed during game start");
                    return;
                }

                // At this point we are in game!
                System.out.println(clientName + " Game with ID " + gameId + " started! Lets win!");

                Thread.sleep(initialTimeOut);

                while (true) {
                    boolean result = waitForRound();
                    if (!result)
                        break;
                    if (logIt)
                        System.out.println(clientName + " Answer from WaitForRound: " + result);

                    if ("LOST".equals(gameStatus)) {
                        System.out.println(clientName + " I LOSSSSSSSSSSSSSSSSSSSSST the game - exiting");
                        LeaveGame();
                        break;
                    }
                    if ("WON".equals(gameStatus)) {
                        System.out.println(clientName + " I WOOONNNNNNNNNNNNN the game - exiting and going to celebrate");
                        LeaveGame();
                        break;
                    }
                    if ("DRAW".equals(gameStatus)) {
                        System.out.println(clientName + " Its the DRAW - exiting. Next time will play better.");
                        LeaveGame();
                        break;
                    }

                    result = getPlayerView();
                    if (!result)
                        break;

                    if ("PLAY".equals(gameStatus)) {
                        Thread.sleep(moveTimeOut);
                        result = MakeMove();
                        if (logIt)
                            System.out.println(clientName + " Answer from MakeMove: " + result);
                        if (!result)
                            break;
                    }

                    if (logIt)
                        System.out.println(clientName + " Answer from playerview: " + result);

                }
            }

        }
        catch (IOException ex) {
            System.err.println("IO exception: " + ex.getMessage());
        }
        catch (RuntimeException ex) {
            System.err.println("Runtime exception: " + ex.getMessage());
            ex.printStackTrace();
        }
        catch (ParseException ex) {
            System.err.println("Parse exception: " + ex.getMessage());
        }
        catch (InterruptedException ex) {
            Logger.getLogger(jsonCommunicator.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    public jsonCommunicator(String client) {
        clientName = client;
    }

    public static int initialTimeOut = 3000;
    public static int moveTimeOut = 500;
    public static boolean logIt = true;
    public static int numberPlayers = 2;
    public static String baseURL = "http://pvs.baltic-amadeus.lt:6798/EnjoyTron/ClientService.svc/";

    public static void main(String[] args) throws InterruptedException {
        System.out.println("Parameters: -u url -p number_players -i intial_timeout -w wait_between_moves -n");
        System.out.println("By default will log to console, if -n provided, logging will not be done");

        System.out.println("Default values: ");
        System.out.println("Initial timeout: " + initialTimeOut);
        System.out.println("Wait timeout: " + moveTimeOut);
        System.out.println("BaseURL: " + baseURL);
        System.out.println("Logging is: " + (logIt ? "ON" : "OFF"));
        System.out.println("Number of players: " + numberPlayers);

        if (args.length > 0) {
            System.out.println("Got parameters!");
            for (int i = 0; i < args.length; i++) {
                switch (args[i]) {
                    case "-n":
                        logIt = false;
                        break;
                    case "-i":
                        if (i + 1 < args.length) {
                            initialTimeOut = Integer.parseInt(args[i + 1]);
                        }
                        break;
                    case "-w":
                        if (i + 1 < args.length) {
                            moveTimeOut = Integer.parseInt(args[i + 1]);
                        }
                        break;
                    case "-u":
                        if (i + 1 < args.length) {
                            baseURL = args[i + 1];
                        }
                        break;
                    case "-p":
                        if (i + 1 < args.length) {
                            numberPlayers = Integer.parseInt(args[i + 1]);
                            if (numberPlayers <= 0)
                                numberPlayers = 1;
                            if (numberPlayers >= 10)
                                numberPlayers = 9;
                        }
                        break;
                }
            }
//            if (args.length >= 1) {
//                try {
//                    initialTimeOut = Integer.parseInt(args[0]);
//                }
//                catch (Exception ex) {
//                    // Just ignoring it - will be default value
//                }
//                System.out.println("Setting new initial timeout to: " + initialTimeOut);
//            }
//            if (args.length >= 2) {
//                try {
//                    moveTimeOut = Integer.parseInt(args[1]);
//                }
//                catch (Exception ex) {
//                    // Just ignoring it - will be default value
//                }
//                System.out.println("Setting new timeout between moves to: " + moveTimeOut);
//            }

        }

        System.out.println("###############");
        System.out.println("Modified values:");

        System.out.println("Initial timeout: " + initialTimeOut);
        System.out.println("Wait timeout: " + moveTimeOut);
        System.out.println("BaseURL: " + baseURL);
        System.out.println("Logging is: " + (logIt ? "ON" : "OFF"));
        System.out.println("Number of players: " + numberPlayers);

        for (int j = 1; j <= numberPlayers; j++) {
            (new Thread(new jsonCommunicator("JavaClient" + j))).start();
        }

        //(new Thread(new jsonCommunicator("JavaClient1"))).start();
        //(new Thread(new jsonCommunicator("JavaClient2"))).start();
        //(new Thread(new jsonCommunicator("Client3"))).start();
        //(new Thread(new jsonCommunicator("Client4"))).start();
    }

}
