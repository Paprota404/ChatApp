import React from "react";
import { useQuery } from "react-query";
import Link from "next/link";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Input } from "@/components/ui/input";

interface Contact {
  userId: string;
  username: string;
  // Add other properties as needed
}

const Contacts: React.FC = () => {
  const [searchTerm, setSearchTerm] = React.useState("");

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
          credentials: "include",
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
      <div className="w-5/6 self-center">
        <Input
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          placeholder="Search contacts"
          className="border-white rounded-3xl mt-5 items-center justify-center flex flex-col"
        />
      </div>

      {status === "success" && (
        <div className="w-5/6 self-center">
          {data
            .filter((contact) =>
              contact.username.toLowerCase().includes(searchTerm.toLowerCase())
            )
            .map((contact) => (
              <Link
                key={contact.userId}
                href={`/chat/${contact.userId}?username=${encodeURIComponent(
                  contact.username
                )}`}
              >
                <a className="border-white flex justify-start h-32 relative gap-4 items-center w-full">
                  <div>
                    <Avatar>
                      <AvatarImage src="OIG2.jpg"></AvatarImage>
                      <AvatarFallback>CN</AvatarFallback>
                    </Avatar>
                  </div>

                  <div className="text-white text-2xl truncate  lg:text-4xl">
                    {contact.username}
                  </div>
                </a>
                <hr className="bg-white w-full mx-auto"></hr>
              </Link>
            ))}
        </div>
      )}
    </>
  );
};

export default Contacts;
