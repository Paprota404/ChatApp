"use client";
import React from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

const AddingContacts = () => {
  const [username, setUsername] = useState("");
  const [errorMessage, setErrorMessage] = useState('');

  async function sendRequest() {
    try {
      const response = await fetch(
        "http://localhost:5108/api/FriendRequest/send",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            receiver_username: username,
          }),
        }
      );

      if (!response.ok) {
        const errorMessage = await response.text();
        setErrorMessage(errorMessage)
        throw new Error(`Failed to send friend request: ${errorMessage}`);
      }

      console.log('Friend request sent successfully');
    } catch (error) {
      console.error('Error sending friend request:', (error as Error).message);
       setErrorMessage((error as Error).message);
    }
  }

  return (
    <Dialog>
      <DialogTrigger className="border-2 w-5/6 self-center mt-5">
        Add new contacts
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle className="mb-4">Add new contact</DialogTitle>
          <Input
            placeholder="Enter username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          ></Input>
          <Button onClick={sendRequest} className="bg-white text-black">Send request</Button>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
};

export default AddingContacts;
