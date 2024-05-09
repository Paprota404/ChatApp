import React from "react";
import { useQuery } from "react-query";
import Link from "next/link";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";


interface Contact {
  userId: string;
  username: string;
  // Add other properties as needed
}

const Contacts: React.FC = () => {
  const getContacts = async (): Promise<Contact[]> => {
    try {
      const headers = {
        "Content-Type": "application/json", 
      };

     
      const response = await fetch(
        "http://localhost:5108/api/Friends/GetFriends",
        {
          method: "GET",
          headers: headers,
          credentials: 'include',
        }
      );

      // Check if the response status is okay
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      // Parse and return the response data
      const data = await response.json();
      return data;
    } catch (error) {
      // Handle the error, log it, or perform any necessary actions
      console.error("An error occurred:", error);
      throw error; // Re-throw the error to propagate it further if needed
    }
  };

  const { data, status } = useQuery<Contact[]>("contacts", getContacts);

  return (
    <>
      {status === "success" && (
        <div>
          {data.map((contact) => (
            <Link key={contact.userId} href={`/chat/${contact.userId}?username=${encodeURIComponent(contact.username)}`}>
              <a className="border-white flex justify-center h-32 items-center w-full">
                <div className="flex relative items-center gap-4">
                  <div>
                    <Avatar>
                      <AvatarImage src="OIG2.jpg"></AvatarImage>
                      <AvatarFallback>CN</AvatarFallback>
                    </Avatar>
                  </div>

                  <div>
                    <div className="text-white text-2xl lg:text-4xl">
                      {contact.username}
                    </div>
                    
                  </div>
                </div>
              </a>

              <hr className="bg-white w-5/6 mx-auto"></hr>
            </Link>
          ))}
        </div>
      )}
    </>
  );
};

export default Contacts;
