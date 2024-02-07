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
import {useRouter} from 'next/navigation';
import {useState} from "react";



const formSchema = z.object({
  email: z.string().email({ message: "Invalid email address" }),
  password: z.string().min(8, {
    message: "Password need to be at least 8 characters long",
  }),
});


//Defining schema
export default function ProfileForm() {
  const router = useRouter();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });


 

  
  const [errorMessage,setError] = useState("");

  async function OnSubmit(values: z.infer<typeof formSchema>) {
      const apiEndpoint =  'http://localhost:5108/api/signup';

      try {
        const response = await fetch(apiEndpoint, {
          method: 'POST',
          headers: {'Content-Type': 'application/json'},
          body: JSON.stringify(values)
        });
    
        // Check if the response contains JSON before trying to parse it
        const contentType = response.headers.get('content-type');
        const isJson = contentType && contentType.includes('application/json');
    
        if (!response.ok) {
          let errorData;
          if (isJson) {
            errorData = await response.json(); // Parse JSON error data
          } else {
            errorData = { message: 'An error occurred during signup.' };
          }
          throw new Error(errorData.message || 'An unknown error occurred.');
        }
    
        // If the response is ok, navigate to the chat page
        router.push("/chat");
      } catch (error) {
        let errorMessage = "An unexpected error occurred.";
        if (error instanceof Error) {
          errorMessage = error.message;
        }
        console.log(errorMessage);
      }
  }
  
  return (
    <div
      className="flex justify-center items-center"
      style={{ backgroundColor: "black", height: "100vh" }}
    >
      <Card className="w-96 p-5">
        <CardTitle className="text-white mb-4">Sign Up</CardTitle>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(OnSubmit)} className="space-y-8">
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="text-white">Enter your email</FormLabel>
                  <FormControl className="text-white">
                    <Input type="email" placeholder="Email" {...field} />
                  </FormControl>
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="text-white">
                    Enter your password
                  </FormLabel>
                  <FormControl className="text-white">
                    <Input type="password" placeholder="Password" {...field} />
                  </FormControl>

                  <FormMessage className="text-white" />
                </FormItem>
              )}
            />

            <div className="flex justify-between">
              <Button className="bg-white hover:bg-gray-500" type="submit">
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
            {errorMessage != "" && <h1>{errorMessage}</h1> }
          </form>
        </Form>
      </Card>
    </div>
  );
}
