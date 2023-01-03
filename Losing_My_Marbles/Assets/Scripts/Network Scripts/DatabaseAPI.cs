using System;
using Firebase.Database;
using Unity.VisualScripting;
using UnityEngine;

public class DatabaseAPI : MonoBehaviour
{
    private static DatabaseReference dbReference;
    
    public static bool hasBeenRestarted;
    
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
    
    public void ListenForGameSession(Action<SessionMessage> callback, Action<AggregateException> fallback)
    {
        var gameSessionID = GameSession.sessionID.ToString();
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson<SessionMessage>(args.Snapshot.GetRawJsonValue()));
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
        
        if (hasBeenRestarted)
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
}