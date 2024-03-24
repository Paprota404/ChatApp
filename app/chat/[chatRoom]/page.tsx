import React from "react";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import {useRouter} from 'next/navigation';

const MessageRoom = () => {
  const router = useRouter();
  //Exploring stuff
  //Figuring out 
  //I probably figured out
  
  

  return (
   
    <> 
     <div
        className="bg-black w-3/4 flex flex-col h-full"
        style={{ marginLeft: "25%" }}
      >
        <div style={{height:"9.5rem"}} className="border-white  border-b-2 flex justify-start  items-center">
          <h1 className="text-white text-5xl tracking-widest items-center gap-5 flex ml-10">
            <Avatar>
              <AvatarImage src="OIG2.jpg"></AvatarImage>
              <AvatarFallback>CN</AvatarFallback>
            </Avatar>
            {roomId}
          </h1>
        </div>
        <div className="flex flex-col-reverse relative gap-5 items-center w-5/6 place-self-center h-full">
      <div className=" w-full flex gap-5  items-center mb-5">
        <Textarea
          className="bg-black text-white rounded-lg"
          placeholder="Write your message"
        ></Textarea>
        <Button className="h-20 text-white border-2 w-20">Send</Button>
      </div>

      <div
        className="place-self-start"
        style={{ maxWidth: "25ch", wordWrap: "break-word" }}
      >
        Ziomal
        <div className="border-2 px-10 text-white py-2">Git</div>
      </div>

      <div
        className="place-self-end mr-24"
        style={{ maxWidth: "25ch", wordWrap: "break-word" }}
      >
        Ja
        <div className="border-2 text-white px-10 py-2">Ok</div>
      </div>
    </div>
        
      </div>
   
    </>
  );
};

export default MessageRoom;
