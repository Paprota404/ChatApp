"use client";
import React from "react";
import AddingContacts from "./AddingContacts";
import PendingRequests from "./PendingRequests";
import Contacts from "./Contacts";
import Default from "./Default";

import { QueryClient, QueryClientProvider } from "react-query";

const layout = ({ children }: { children: React.ReactNode }) => {
  const queryClient = new QueryClient();
  return (
    <QueryClientProvider client={queryClient}>
      <>
      <div className="w-1/4 h-full flex flex-col absolute  border-r-2 bg-black">
        <div className="  flex flex-col justify-center h-32 items-center w-full">
          <h1 className="text-white text-xl sm:text-2xl md:text-3xl lg:text-4xl xl:text-5xl tracking-widest">DirectMe</h1>
        
        </div>
       

        <Contacts />
        <AddingContacts />
        <PendingRequests />
      </div>
      <Default></Default>
      {children}
      </>
    </QueryClientProvider>
  );
};

export default layout;
