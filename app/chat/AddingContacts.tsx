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
    let token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjljNTQyYmVkLWVlOGEtNGZmZi05NjAwLTAxYTdjNmNlNTg3ZSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJQYXByb3RhIiwiZXhwIjoxNzA4NjQwMjAyLCJpc3MiOiJEaXJlY3RNZSIsImF1ZCI6IkRpcmVjdGVycyJ9.z712esJbddUlLHwTe8Qod5vpeLXi0FUgC8Qr1FkuLhw";
    try {
      const response = await fetch(
        "http://localhost:5108/api/FriendRequest/send",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: 'include',
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
