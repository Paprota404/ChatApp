"use client";
import React, {useEffect, useRef} from "react";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { useSearchParams } from "next/navigation";
import * as signalR from "@microsoft/signalr";

const MessageRoom = () => {
  const searchParams = useSearchParams();
  const username = searchParams.get("username");

  const connection = useRef<signalR.HubConnection | null>(null);

 useEffect(() => {
    // Assign the new connection to the .current property of the ref
    connection.current = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5108/ChatHub", {
        accessTokenFactory: (): string | Promise<string> => {
          // Retrieve the access token from wherever it's stored (e.g., local storage)
          const token = localStorage.getItem("jwtToken");
          return token || ""; // Return an empty string if the token is null
        },
      })
      .build();

    // Start the connection
    if (connection.current) {
      connection.current.start()
        .then(() => {
            console.log("Connection started");

            // Set up a message handler after the connection is established
            if (connection.current) {
              connection.current.on("ReceiveMessage", (senderId, message) => {
                 console.log(`Received message from ${senderId}:`, message);
                 // Handle the message here
              });
            }
        })
        .catch((err) => console.error("Error while starting connection: " + err));
    }

    // Cleanup function to stop the connection when the component unmounts
    return () => {
      if (connection.current) {
        connection.current.stop();
      }
    };
 }, []); // Empty dependency array ensures this runs once on mount

 // Function to send a message to a specific user
 function sendMessageToUser(recipientUserId: string, message: string): void {
    if(connection.current) {
      connection.current.invoke("SendMessage", recipientUserId, message)
        .catch((err) => console.error(err));
    }
 }

 sendMessageToUser("2a97463e-1340-4f9d-84ea-14667929de05","Ja cie lubie");

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
              className="bg-black text-white rounded-lg"
              placeholder="Write your message"
            ></Textarea>
            <Button className="h-20 text-white border-2 w-20">Send</Button>
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
