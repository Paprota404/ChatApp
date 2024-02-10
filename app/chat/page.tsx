import React from "react";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Textarea } from "@/components/ui/textarea";

const Chat = () => {
  return (
    <>
      <div className="w-1/4 h-full flex flex-col absolute  border-r-2 bg-black">
        <div className="border-white  border-b-2 flex justify-center h-32 items-center w-full">
          <h1 className="text-white text-5xl tracking-widest">DirectMe</h1>
        </div>

        <div className="border-white  flex justify-start h-32 items-center w-full">
          <div className="flex relative items-center left-10 gap-4">
            <div>
              <Avatar>
                <AvatarImage src="https://historia.org.pl/wp-content/uploads/2011/12/facebookavatar.jpg"></AvatarImage>
                <AvatarFallback>CN</AvatarFallback>
              </Avatar>
            </div>

            <div>
              <div className="text-white text-2xl">Karolina</div>
              <div className="text-gray-400">Tak</div>
            </div>
          </div>
        </div>
        <hr className="bg-white w-5/6 mx-auto"></hr>
      </div>

      <div className="bg-black w-3/4 flex flex-col h-full" style={{ marginLeft: "25%" }}>
        <div className="border-white border-b-2 flex justify-start h-32 items-center w-full">
          <h1 className="text-white text-5xl tracking-widest items-center gap-5 flex ml-10">
            <Avatar>
              <AvatarImage src="https://historia.org.pl/wp-content/uploads/2011/12/facebookavatar.jpg"></AvatarImage>
              <AvatarFallback>CN</AvatarFallback>
            </Avatar>
            Karolina
          </h1>
        </div>
        <div className="">
          <Textarea></Textarea>
        </div>
      </div>
    </>
  );
};

export default Chat;
