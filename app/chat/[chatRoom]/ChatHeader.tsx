import React from 'react';
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import Image from "next/image";
import { useSearchParams} from "next/navigation";

const ChatHeader = () => {

    const searchParams = useSearchParams();
  const username = searchParams.get("username");
  return (
    <div
    style={{ height: "9.5rem" }}
    className="border-white z-50  border-b-2 flex justify-between  items-center"
  >
    <h1 className="text-white text-2xl sm:text-3xl md:text-4xl lg:text-5xl tracking-widest items-center gap-5 flex ml-10">
      <Avatar>
        <AvatarImage src="OIG2.jpg"></AvatarImage>
        <AvatarFallback>CN</AvatarFallback>
      </Avatar>
      {username}
    </h1>

    <button className="mr-10">
      <Image
        src="/chess-icon.svg"
        width={100}
        height={100}
        alt="Send"
        className="w-10 h-10"
      />
    </button>
  </div>
  )
}

export default ChatHeader