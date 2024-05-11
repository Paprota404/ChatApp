import React from "react";
import AddingContacts from "./AddingContacts";
import PendingRequests from "./PendingRequests";
import Contacts from "./Contacts";

const Sidebar = () => {
  return (
    <div className="w-1/4 h-full flex flex-col absolute overflow-y-scroll border-r-2 bg-black sidebar">
      <div className="  flex flex-col justify-center h-32 items-center w-full">
        <h1 className="text-white text-xl sm:text-2xl md:text-3xl lg:text-4xl xl:text-5xl tracking-widest">
          DirectMe
        </h1>
      </div>
      <Contacts />
      <AddingContacts />
      <PendingRequests />
    </div>
  );
};

export default Sidebar;
