using System;
using Firebase.Database;
using UnityEngine;

public class DatabaseAPI : MonoBehaviour
{
    private DatabaseReference dbReference;

    private void Awake()
    {
        // Gets an instance of the database
        FirebaseDatabase.GetInstance("https://losing-my-marbles-620eb-default-rtdb.europe-west1.firebasedatabase.app/");
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        dbReference.SetValueAsync(null); //clears the database every play session
        dbReference.SetValueAsync("movement"); //makes sure we can post to movement
    }

    public void PostActions(ActionMessage actionMessage, Action callback, Action<AggregateException> fallback)
    {
        var actionJson = JsonUtility.ToJson(actionMessage);
        dbReference.Child("movement").Push().SetRawJsonValueAsync(actionJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void ListenForActions(Action<ActionMessage> callback, Action<AggregateException> fallback)
    {
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson<ActionMessage>(args.Snapshot.GetRawJsonValue()));
        }

        dbReference.Child("movement").ChildAdded += CurrentListener;
    }
}
