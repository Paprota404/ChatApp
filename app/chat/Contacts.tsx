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
      // Retrieve JWT token from local storage
      const jwtToken = localStorage.getItem("jwtToken");

      console.log(jwtToken);

      // Set up headers with Authorization header containing the token
      const headers = {
        Authorization: `Bearer ${jwtToken}`,
        "Content-Type": "application/json", // Adjust content type as needed
      };

      // Make the fetch request with the headers
      const response = await fetch(
        "http://localhost:5108/api/Friends/GetFriends",
        {
          method: "GET",
          headers: headers,
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
              <a className="border-white flex justify-start h-32 items-center w-full">
                <div className="flex relative items-center left-10 gap-4">
                  <div>
                    <Avatar>
                      <AvatarImage src="OIG2.jpg"></AvatarImage>
                      <AvatarFallback>CN</AvatarFallback>
                    </Avatar>
                  </div>

                  <div>
                    <div className="text-white text-2xl">
                      {contact.username}
                    </div>
                    <div className="text-gray-400">Tak</div>
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
