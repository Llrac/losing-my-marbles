using System;
using Firebase.Database;
using Unity.VisualScripting;
using UnityEngine;

public class DatabaseAPI : MonoBehaviour
{
    private static DatabaseReference dbReference;
    
    public static bool hasBeenRestarted;
    public static bool mobileIsListening;

    private static DatabaseAPI instance;
    public static DatabaseAPI Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Gets an instance of the database
        FirebaseDatabase.GetInstance("https://losing-my-marbles-620eb-default-rtdb.europe-west1.firebasedatabase.app/");
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        // dbReference.SetValueAsync(null); //clears the database every play session
        // dbReference.SetValueAsync("game session");
        // dbReference.SetValueAsync("new hand");
        // dbReference.SetValueAsync("movement"); //makes sure we can post to movement

    }

    public void CreateGameSession(GameSessionMessage gameSessionMessage, Action callback, Action<AggregateException> fallback)
    {
        var gameSessionID = GameSession.sessionID.ToString();
        var gameSessionJson = JsonUtility.ToJson(gameSessionMessage);
        dbReference.Child("game session").Child(gameSessionID).Push().SetRawJsonValueAsync(gameSessionJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void JoinGameSession(SessionMessage sessionMessage, Action callback, Action<AggregateException> fallback)
    {
        var gameSessionID = sessionMessage.gameSessionID.ToString();
        var joinGameJson = JsonUtility.ToJson(sessionMessage);

        dbReference.Child("game session").Child(gameSessionID).Push().SetRawJsonValueAsync(joinGameJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }
    
    public void PostGameSessionInfoToMobile(GameSessionMessage gameSessionMessage, Action callback, Action<AggregateException> fallback)
    {
        var gameSessionID = GameSession.sessionID.ToString();
        var playerID = PlayerID.playerID.ToString();
        var postGameJson = JsonUtility.ToJson(gameSessionMessage);

        dbReference.Child("game session").Child(gameSessionID).Child(playerID).Push().SetRawJsonValueAsync(postGameJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }
    public void QuitGameSessionOnMobile(GameSessionMessage gameSessionMessage, Action callback, Action<AggregateException> fallback)
    {
        var gameSessionID = gameSessionMessage.gameSessionID.ToString();
        var joinGameJson = JsonUtility.ToJson(gameSessionMessage);

        dbReference.Child("game session").Child(gameSessionID).Push().SetRawJsonValueAsync(joinGameJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }
    
    public void ListenForGameSession(Action<SessionMessage> callback, Action<AggregateException> fallback)
    {
        
        var gameSessionID = GameSession.sessionID.ToString();
        var playerID = PlayerID.playerID.ToString();
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson<SessionMessage>(args.Snapshot.GetRawJsonValue()));
        }

        dbReference.Child("game session").Child(gameSessionID).Child(playerID).ChildAdded += CurrentListener;
    }
    
    public void ListenForGameSessionMobile(Action<GameSessionMessage> callback, Action<AggregateException> fallback)
    {
        // if (mobileIsListening)
        // {
        //     Debug.Log("don't add again");
        //     return;
        // }
        
        var gameSessionID = GameSession.sessionID.ToString();
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson<GameSessionMessage>(args.Snapshot.GetRawJsonValue()));
        }
            
        dbReference.Child("game session").Child(gameSessionID).ChildAdded += CurrentListener;
        
    }
    
    
    
    public void PostActions(ActionMessage actionMessage, Action callback, Action<AggregateException> fallback)
    {
        var gameSessionID = GameSession.sessionID.ToString();
        var actionJson = JsonUtility.ToJson(actionMessage);
        dbReference.Child("game session").Child(gameSessionID).Push().SetRawJsonValueAsync(actionJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void ListenForActions(Action<ActionMessage> callback, Action<AggregateException> fallback)
    {
        var gameSessionID = GameSession.sessionID.ToString();
        
        if (hasBeenRestarted || mobileIsListening)
        {
            Debug.Log("dont add again");
            return;
        }

        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson<ActionMessage>(args.Snapshot.GetRawJsonValue()));
        }

        dbReference.Child("game session").Child(gameSessionID).ChildAdded += CurrentListener;
    }

    public void PostNewHand(NewHandMessage newHandMessage, Action callback, Action<AggregateException> fallback)
    {
        var gameSessionID = GameSession.sessionID.ToString();
        
        var newHandJson = JsonUtility.ToJson(newHandMessage);
        dbReference.Child("game session").Child(gameSessionID).Push().SetRawJsonValueAsync(newHandJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void ListenForNewHand(Action<NewHandMessage> callback, Action<AggregateException> fallback)
    {
        var gameSessionID = GameSession.sessionID.ToString();

        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson<NewHandMessage>(args.Snapshot.GetRawJsonValue()));
        }

        dbReference.Child("game session").Child(gameSessionID).ChildAdded += CurrentListener;

    }

    public void DeleteGameSession()
    {
        var gameSessionID = GameSession.sessionID.ToString();
        dbReference.Child("game session").Child(gameSessionID).RemoveValueAsync();
    }
    
}