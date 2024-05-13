"use client";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { Card, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import "../globals.css";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useRouter } from "next/navigation";
import { useState } from "react";

const formSchema = z.object({
  username: z.string().min(1, { message: "Username must be at least 1 character long" }).max(20, { message: "Username must be at most 20 characters long" }),
  password: z.string().min(8, { message: "Password need to be at least 8 characters long" }),
});

//Defining schema
export default function ProfileForm() {
  const router = useRouter();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      username: "",
      password: "",
    },
  });

  const [errorMessage, setError] = useState("");
  const [isSigning, setSigning] = useState(false);

  async function OnSubmit(values: z.infer<typeof formSchema>) {
    const apiEndpoint = "http://localhost:5108/api/signup";

    setSigning(true);

    try {
      const response = await fetch(apiEndpoint, {
        mode: "cors",
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          UserName: values.username,
          Password: values.password,
        }),
      });

      if (response.status === 200) {
      

        setTimeout(() => {
          router.push("/login");
        }, 1000);
      } else if (response.status === 400) {
       
        let errorData = await response.json();
        throw new Error(errorData.message || "Bad Request");
      } else if (response.status === 500) {
        setError("Please try again");
      } else if (response.status === 409) {
        setError("User with this usernam already exists");
        setSigning(false);
      } else {
        
        setSigning(false);
        throw new Error(`Request failed with status ${response.status}`);
      }
    } catch (error) {
      setSigning(false);
      setError("An unexpected error occurred:" + (error as Error).message);
    }
  }

  return (
    <div
      className="flex justify-center items-center"
      style={{ backgroundColor: "black", height: "100vh" }}
    >
      <Card className="w-96 p-5">
        <CardTitle className="text-white flex-col justify-between mb-4">
          <div className="flex justify-between">
          Sign Up 
          {isSigning && <div>Signing up...</div>}
          </div>
          
        </CardTitle>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(OnSubmit)} className="space-y-8">
            <FormField
              control={form.control}
              name="username"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="text-white">
                    Enter your username
                  </FormLabel>
                  <FormControl className="text-white">
                    <Input type="username"  className="rounded-xl"  placeholder="Username" {...field} />
                  </FormControl>

                  <FormMessage className="text-white" />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              
              name="password"
              render={({ field }) => (
                <FormItem >
                  <FormLabel className="text-white">
                    Enter your password
                  </FormLabel>
                  <FormControl className="text-white">
                    <Input type="password" className="rounded-xl" placeholder="Password" {...field} />
                  </FormControl>

                  <FormMessage className="text-white" />
                </FormItem>
              )}
            />

            <div className="flex justify-between">
              <Button
                className="bg-white text-black rounded-xl hover:bg-gray-500"
                type="submit"
              >
                Sign Up
              </Button>
              <a
                href="/login"
                className="text-white cursor-pointer justify-end"
                type="submit"
              >
                I already have an account
              </a>
            </div>
            {errorMessage != "" && (
              <h1 className="text-white">{errorMessage}</h1>
            )}
            <div className="text-lg font-light">Please note that your first signup might experience a delay due to Azure&apos;s cold start.</div>
          </form>

        </Form>
      </Card>
    </div>
  );
}
