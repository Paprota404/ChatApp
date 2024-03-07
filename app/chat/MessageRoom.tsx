import React from 'react';
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";

const MessageRoom = () => {
  return (
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
            <div className="border-2 text-white px-10 py-2">
              Ok
            </div>
          </div>
        </div>
  )
}

export default MessageRoom