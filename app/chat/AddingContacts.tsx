"use client";
import React from "react";
import { Button } from "@/components/ui/button";
import { useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"


const AddingContacts = () => {
  const [username, setUsername] = useState("");
  const [Message, setMessage] = useState('');


  async function sendRequest() {
   
    try {

      const response = await fetch(
        "https://directmechat.azurewebsites.net/FriendRequest/FriendRequest/send",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          credentials:'include',
          body: JSON.stringify(username),
        }
      );

      if (!response.ok) {
        const errorMessage = await response.text();
        setMessage(errorMessage)
       
      }else{
        setMessage("Friend request sent successfully!");
      }

      
    } catch (error) {
      console.error('Error sending friend request:', (error as Error).message);
      setMessage((error as Error).message);
    }
  }

  return (
    
    <Dialog>
      
      <DialogTrigger className="border-2 w-5/6 rounded-xl self-center mt-5">
        Add new friends
      </DialogTrigger>
      
      <DialogContent >
     
        <DialogHeader >
          
          <DialogTitle className="mb-4 text-2xl">Add new friends</DialogTitle>
          <Input className="rounded-xl"
            placeholder="Enter username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          ></Input>
          <Button onClick={sendRequest} className="bg-white text-black rounded-xl">Send request</Button>
          {Message != "" && <h1 className="text-white">{Message}</h1> }
        </DialogHeader>
       
      </DialogContent>
     
    </Dialog>
    
  );
};

export default AddingContacts;