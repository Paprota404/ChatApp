'use client'
import React from "react";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import MessageRoom from "./MessageRoom";
import AddingContacts from "./AddingContacts";
import PendingRequests from "./PendingRequests";
import Contacts from "./Contacts";
import { QueryClient, QueryClientProvider } from 'react-query';


const Chat = () => {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
    <>
      <div className="w-1/4 h-full flex flex-col absolute  border-r-2 bg-black">
        <div className="border-white   flex justify-center h-32 items-center w-full">
          <h1 className="text-white text-5xl tracking-widest">DirectMe</h1>
        </div>

        <div className="border-white bg-gray-600  flex justify-start h-32 items-center w-full">
          <div className="flex relative items-center left-10 gap-4">
            <div>
              <Avatar>
                <AvatarImage src="https://historia.org.pl/wp-content/uploads/2011/12/facebookavatar.jpg"></AvatarImage>
                <AvatarFallback>CN</AvatarFallback>
              </Avatar>
            </div>

            <div>
              <div className="text-white text-2xl">Karolina</div>
              <div className="text-gray-400">Tak</div>
            </div>
          </div>
        </div>
        <hr className="bg-white w-5/6 mx-auto"></hr>
        <div className="border-white  flex justify-start h-32 items-center w-full">
          <div className="flex relative items-center left-10 gap-4">
            <div>
              <Avatar>
                <AvatarImage src="https://historia.org.pl/wp-content/uploads/2011/12/facebookavatar.jpg"></AvatarImage>
                <AvatarFallback>CN</AvatarFallback>
              </Avatar>
            </div>

            <div>
              <div className="text-white text-2xl">Karolina</div>
              <div className="text-gray-400">Tak</div>
            </div>
          </div>
        </div>
        <hr className="bg-white w-5/6 mx-auto"></hr>

        <Contacts />
        <AddingContacts />
        <PendingRequests />
        
      </div>

      <div
        className="bg-black w-3/4 flex flex-col h-full"
        style={{ marginLeft: "25%" }}
      >
        <div className="border-white border-b-2 flex justify-start h-36  items-center">
          <h1 className="text-white text-5xl tracking-widest items-center gap-5 flex ml-10">
            <Avatar>
              <AvatarImage src="https://historia.org.pl/wp-content/uploads/2011/12/facebookavatar.jpg"></AvatarImage>
              <AvatarFallback>CN</AvatarFallback>
            </Avatar>
            Karolina
          </h1>
        </div>
        <MessageRoom></MessageRoom>
      </div>
    </>
    </QueryClientProvider>
  );
};

export default Chat;
