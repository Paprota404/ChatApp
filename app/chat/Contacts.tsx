import React from 'react'
import {useQuery} from 'react-query';

interface Contact {
  UserId: string;
  Username: string;
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
          "Authorization": `Bearer ${jwtToken}`,
          'Content-Type': 'application/json', // Adjust content type as needed
      };

      // Make the fetch request with the headers
      const response = await fetch("http://localhost:5108/api/Friends/GetFriends", {
          method: 'GET',
          headers: headers
      });

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
  }

  const { data, status, error } = useQuery<Contact[]>("contacts", getContacts);


  return (
    <>
      {status === 'loading' && <div>Loading...</div>}
      {status === 'error' && <div>Error: {(error as Error).message}</div>}
      {status === 'success' && (
        <div>
          {data.map((contact) => (
            <div key={contact.UserId}>
              {contact.Username}
            </div>
          ))}
        </div>
      )}
    </>
  );
}


export default Contacts