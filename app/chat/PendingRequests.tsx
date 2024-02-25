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

const PendingRequests = () => {
  let token ="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjljNTQyYmVkLWVlOGEtNGZmZi05NjAwLTAxYTdjNmNlNTg3ZSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJQYXByb3RhIiwiZXhwIjoxNzA4ODYzMTA4LCJpc3MiOiJEaXJlY3RNZSIsImF1ZCI6IkRpcmVjdGVycyJ9.8uhR0dthGXx92Q-5kymQFqllzNM-TVmBVnuzpFGrGsY";

  return (
    <Dialog>
      <DialogTrigger className="border-2 w-5/6 self-center mt-5">
        Pending requests: 0
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle className="mb-4">Pending Requests: 0</DialogTitle>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
};

export default PendingRequests;
