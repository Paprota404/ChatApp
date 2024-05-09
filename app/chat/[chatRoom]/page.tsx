"use client";
import React, { useEffect, useRef, useState } from "react";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { useSearchParams, useParams } from "next/navigation";
import * as signalR from "@microsoft/signalr";
import data from "@emoji-mart/data";
import Picker from "@emoji-mart/react";

const MessageRoom = () => {
  const searchParams = useSearchParams();
  const username = searchParams.get("username");
  const params = useParams();
  const chatId = params.chatRoom;
  const [messages, setMessages] = useState<Message[]>([]);

  interface Message {
    content: string;
    senderId: string;
    sentAt: Date;
    id: number;
  }

  const connection = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    const startConnection = async () => {
      // Assign the new connection to the .current property of the ref
      connection.current = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5108/ChatHub")
        .configureLogging("information")
        .build();

      try {
        await connection.current.start();
        console.log("Connection started");

        if (connection.current) {
          await connection.current.invoke("StartOneToOneSession", chatId);

          connection.current.on("ReceiveMessage", (message) => {
            console.log(message);

            setMessages((prevMessages) => [...prevMessages, message]);
          });

          connection.current.on("ReceiveMessages", (messages: Message[]) => {
            console.log("Received all messages:", messages);
            setMessages(messages);
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
  }, [chatId]);

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
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setMessage(event.target.value);
  };

  const handleEmojiSelect = (emoji) => {
    // Append the emoji to the current message text
    setMessage(message + emoji.colons);
  };

  const handleSubmit = () => {
    if (message.length == 0) {
      return;
    }
    const chatIdString = Array.isArray(chatId) ? chatId.join("") : chatId;
    console.log(chatIdString, message);

    sendMessageToUser(chatIdString, message);
    setMessage("");
  };

  useEffect(() => {
    document.body.classList.add("dynamic-page-active");

    return () => {
      document.body.classList.remove("dynamic-page-active");
    };
  }, []);

  return (
    <>
      <div className="bg-black w-3/4 flex flex-col h-full dynamic-page active">
        <div
          style={{ height: "9.5rem" }}
          className="border-white z-50  border-b-2 flex justify-start  items-center"
        >
          <h1 className="text-white text-2xl sm:text-3xl md:text-4xl lg:text-5xl tracking-widest items-center gap-5 flex ml-10">
            <Avatar>
              <AvatarImage src="OIG2.jpg"></AvatarImage>
              <AvatarFallback>CN</AvatarFallback>
            </Avatar>
            {username}
          </h1>
        </div>
        <div className="flex flex-col-reverse relative gap-2 items-center w-full place-self-center h-full overflow-y-scroll">
          {/* <Picker  data={data} onEmojiSelect={console.log} /> */}
          <div className=" w-5/6 flex gap-5 items-center mb-5">
           
            <div className="relative w-full">
              <input
                type="text"
                value={message}
                onChange={handleTextareaChange}
                placeholder="Type your message..."
                className="w-full p-5  border border-white bg-black h-12 focus:outline-none rounded-3xl"
              />
              <button className="absolute right-2 top-1 bg-blue-500 text-white py-2 px-4 rounded-md">
                Send
              </button>
            </div>

            <Button
              onClick={handleSubmit}
              className="h-12 text-white border-2 rounded-full"
            >
              Send
            </Button>
          </div>

          <div className="w-5/6 flex-col">
            {messages
              .slice()

              .map((message, index) => {
                // Parse the ISO 8601 string into a Date object
                const sentAtDate = new Date(message.sentAt);

                // Extract the hours and minutes
                const day = sentAtDate.getDate().toString().padStart(2, "0");
                const month = (sentAtDate.getMonth() + 1)
                  .toString()
                  .padStart(2, "0");
                const hours = sentAtDate.getHours().toString().padStart(2, "0");
                const minutes = sentAtDate
                  .getMinutes()
                  .toString()
                  .padStart(2, "0");

                return (
                  <div
                    key={index}
                    style={{ maxWidth: "17ch", wordWrap: "break-word" }}
                    className={`${
                      message.senderId === chatId ? "" : "ml-auto"
                    }`}
                  >
                    <div className="border-2 px-10 rounded-xl text-white py-2 my-2">
                      {message.content}
                    </div>
                    <div className="text-xs text-gray-500 text-right">
                      {`${day}/${month} ${hours}:${minutes}`}
                    </div>
                  </div>
                );
              })}
          </div>
        </div>
      </div>
    </>
  );
};

export default MessageRoom;
