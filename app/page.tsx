import Image from "next/image";
import Link from "next/link";

export default function Home() {
  return (
    <main className="bg-black h-screen w-screen justify-center flex flex-col items-center">
      <Image src="/OIG2.jpg" height={1000} width={400} alt="Logo"></Image>
      <div className="flex gap-4">
      <Link href="/signup" className="border-2 p-5 rounded-xl text-2xl">Sign up</Link>
      <Link href="/login" className="border-2 p-5 rounded-xl text-2xl">Login</Link>
      </div>
    </main>
  );
}
