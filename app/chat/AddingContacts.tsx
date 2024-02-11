import React from 'react';
import { Input } from "@/components/ui/input";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";


const AddingContacts = () => {
  return (
    <Dialog>
    <DialogTrigger className="border-2 w-5/6 self-center mt-5">Add new contacts</DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle className="mb-4">Add new contact</DialogTitle>
        <Input placeholder="Enter username"></Input>
      </DialogHeader>
    </DialogContent>
  </Dialog>
  )
}

export default AddingContacts