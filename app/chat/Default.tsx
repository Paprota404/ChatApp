import React from 'react'
import {useLocation} from "react-router-dom";

const Default = () => {
    const location = useLocation();
    const showDiv = location.pathname === '/chat';
  return (
    <>
    {showDiv && (
      <div
        className="bg-black w-3/4 flex flex-col h-full"
        style={{ marginLeft: "25%" }}
      >
        {/* Your div content */}
      </div>
    )}
    {/* Other components or content for the chat list */}
  </>
  )
}

export default Default