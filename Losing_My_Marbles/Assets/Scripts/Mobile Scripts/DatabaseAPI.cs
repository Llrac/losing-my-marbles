using System;
using System.Collections;
using System.Collections.Generic;
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

    public void PostMove(MoveMessage moveMessage, Action callback, Action<AggregateException> fallback)
    {
        var moveJson = JsonUtility.ToJson(moveMessage);
        dbReference.Child("movement").Push().SetRawJsonValueAsync(moveJson).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void ListenForMovement(Action<MoveMessage> callback, Action<AggregateException> fallback)
    {
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null) fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
            else callback(JsonUtility.FromJson<MoveMessage>(args.Snapshot.GetRawJsonValue()));
        }

        dbReference.Child("movement").ChildAdded += CurrentListener;
    }
}
