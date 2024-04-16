"use client";
import Default from "./Default";
import Sidebar from "./Sidebar";
import React, { useEffect } from 'react';
import { useRouter } from 'next/navigation';

import { QueryClient, QueryClientProvider } from "react-query";

const Layout = ({ children }: { children: React.ReactNode }) => {
  const queryClient = new QueryClient();

  const router = useRouter();

 useEffect(() => {
    const checkAuth = async () => {
      

      try {
        const response = await fetch('https://directme.azurewebsites.net/api/isAuthenticated/check', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'credentials':'include',
          },
        });

        if (!response.ok) {
          throw new Error('Network response was not ok');
        }

        const data = await response.json();
        console.log(data);

        if (!data.isAuthenticated) {
          router.push("/login");
        }
      } catch (error) {
        console.error('There has been a problem with your fetch operation:', error);
        // Optionally, redirect to login if there's an error or if the user is not authenticated
        router.push("/login");
      }
    };

    checkAuth();
 }, [router]);
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

export default Layout;
