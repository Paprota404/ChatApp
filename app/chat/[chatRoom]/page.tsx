"use client";
import React, { useEffect, useRef, useState } from "react";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { useSearchParams, useParams } from "next/navigation";
import * as signalR from "@microsoft/signalr";

const MessageRoom = () => {
  const searchParams = useSearchParams();
  const username = searchParams.get("username");
  const params = useParams();
  const chatId = params.chatRoom;
  console.log(chatId);

  const connection = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    const startConnection = async () => {
      // Assign the new connection to the .current property of the ref
      connection.current = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5108/ChatHub", {
          accessTokenFactory: (): string | Promise<string> => {
            // Retrieve the access token from wherever it's stored (e.g., local storage)
            const token = localStorage.getItem("jwtToken");
            console.log(token, "token");
            return token || ""; // Return an empty string if the token is null
          },
        })
        .configureLogging("information")
        .build();

      try {
        // Start the connection
        await connection.current.start();
        console.log("Connection started");

        if (connection.current) {
          // Invoke the StartOneToOneSession method
          await connection.current.invoke("StartOneToOneSession", chatId);

          // Set up a message handler after the connection is established
          connection.current.on("ReceiveMessage", (senderId, message) => {
            console.log(`Received message from ${senderId}:`, message);
            // Handle the message here
          });

          connection.current.on("ReceiveMessages", (messages) => {
            console.log("Received all messages:", messages);
            // Handle the messages here
          });

          connection.current
            .invoke("GetLatestMessages", chatId)
            .catch((err) => console.log(err));
        }
      } catch (err) {
        console.error(
          "Error while starting connection or invoking method: " + err
        );
      }
    };

    startConnection();

    // Cleanup function to stop the connection when the component unmounts
    return () => {
      if (connection.current) {
        connection.current.stop();
      }
    };
  }, []);
  // Function to send a message to a specific user
  function sendMessageToUser(recipientUserId: string, message: string): void {
    if (connection.current) {
      connection.current
        .invoke("SendMessage", recipientUserId, message)
        .catch((err) => console.error(err));
    }
  }

  const [message, setMessage] = useState("");

  const handleTextareaChange = (
    event: React.ChangeEvent<HTMLTextAreaElement>
  ) => {
    setMessage(event.target.value);
  };

  const handleSubmit = () => {
    const chatIdString = Array.isArray(chatId) ? chatId.join("") : chatId;
    console.log(chatIdString, message);
    sendMessageToUser(chatIdString, message);
  };

  return (
    <>
      <div
        className="bg-black w-3/4 flex flex-col h-full"
        style={{ marginLeft: "25%" }}
      >
        <div
          style={{ height: "9.5rem" }}
          className="border-white z-50  border-b-2 flex justify-start  items-center"
        >
          <h1 className="text-white text-5xl tracking-widest items-center gap-5 flex ml-10">
            <Avatar>
              <AvatarImage src="OIG2.jpg"></AvatarImage>
              <AvatarFallback>CN</AvatarFallback>
            </Avatar>
            {username}
          </h1>
        </div>
        <div className="flex flex-col-reverse relative gap-5 items-center w-5/6 place-self-center h-full">
          <div className=" w-full flex gap-5  items-center mb-5">
            <Textarea
              onChange={handleTextareaChange}
              className="bg-black text-white rounded-lg"
              placeholder="Write your message"
            ></Textarea>
            <Button
              onClick={handleSubmit}
              className="h-20 text-white border-2 w-20"
            >
              Send
            </Button>
          </div>

          <div
            className="place-self-start"
            style={{ maxWidth: "25ch", wordWrap: "break-word" }}
          >
            Ziomal
            <div className="border-2 px-10 text-white py-2">Git</div>
          </div>

          <div
            className="place-self-end mr-24"
            style={{ maxWidth: "25ch", wordWrap: "break-word" }}
          >
            Ja
            <div className="border-2 text-white px-10 py-2">Ok</div>
          </div>
        </div>
      </div>
    </>
  );
};

export default MessageRoom;
