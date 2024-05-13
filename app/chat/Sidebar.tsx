import React from "react";
import AddingContacts from "./AddingContacts";
import PendingRequests from "./PendingRequests";
import Contacts from "./Contacts";

const Sidebar = () => {
  return (
    <div className="w-1/4 h-full flex flex-col absolute overflow-y-scroll border-r-2 bg-black sidebar">
      <div className="tracking-widest text-white text-4xl xl:text-5xl flex flex-col justify-center min-h-32 h-32 items-center w-full">     
          DirectMe
      </div>
      <Contacts />
      <AddingContacts />
      <PendingRequests />
    </div>
  );
};

export default Sidebar;
