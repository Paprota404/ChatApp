"use client";
import React from "react";
import Default from "./Default";
import Sidebar from "./Sidebar";

import { QueryClient, QueryClientProvider } from "react-query";

const layout = ({ children }: { children: React.ReactNode }) => {
  const queryClient = new QueryClient();
  return (
    <QueryClientProvider client={queryClient}>
      <>
      <Sidebar></Sidebar>
      <Default></Default>
      {children}
      </>
    </QueryClientProvider>
  );
};

export default layout;
