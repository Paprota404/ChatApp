"use client";
import React from "react";
import MessageRoom from "./[roomId]";
import AddingContacts from "./AddingContacts";
import PendingRequests from "./PendingRequests";
import Contacts from "./Contacts";
import { QueryClient, QueryClientProvider } from "react-query";
import {useRouter} from 'next/navigation';
import Default from './Default';

const Chat = () => {
  const queryClient = new QueryClient();
  const router = useRouter();
  const {roomID} = router.query;

  return (
    <QueryClientProvider client={queryClient}>
     
      <>
       
          <div className="w-1/4 h-full flex flex-col absolute  border-r-2 bg-black">
            <div className="border-white   flex justify-center h-32 items-center w-full">
              <h1 className="text-white text-5xl tracking-widest">DirectMe</h1>
            </div>
            <hr
              style={{ height: "2px" }}
              className="bg-white w-full  mx-auto"
            ></hr>

            <Contacts />
            <AddingContacts />
            <PendingRequests />
          </div>

          
      
        

          
       
      </>
      
    </QueryClientProvider>
  );
};

export default Chat;
