import React from 'react';


const Chat = () => {


  return (
    <>
    <div className="w-1/4 h-96 flex flex-col bg-black">
        <input className="absolute bottom-0" type="text" id="message" placeholder="Enter your message"></input>
    </div>

    </>
  )
}

export default Chat