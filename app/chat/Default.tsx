import React from "react";
import Image from "next/image";

const Default = () => {
  return (
    <>
      <div
        className="bg-black absolute w-3/4 flex flex-col h-full justify-center items-center"
        style={{ marginLeft: "25%" }}
      >
        <Image className="w-1/3" src="/OIG2.jpg" height={800} width={300} alt="Logo"></Image>
      </div>
    </>
  );
};

export default Default;
