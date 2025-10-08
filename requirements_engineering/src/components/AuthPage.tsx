import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "./ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "./ui/tabs";
import { LoginForm } from "./LoginForm";
import { SignUpForm } from "./SignUpForm";

export function AuthPage() {
  const [activeTab, setActiveTab] = useState<"login" | "signup">("login");

  return (
    <Card className="w-full max-w-md mx-4">
      <CardHeader className="text-center">
        <CardTitle>Welcome</CardTitle>
        <CardDescription>
          {activeTab === "login" 
            ? "Sign in to your account to continue" 
            : "Create an account to get started"}
        </CardDescription>
      </CardHeader>
      <CardContent>
        <Tabs value={activeTab} onValueChange={(value) => setActiveTab(value as "login" | "signup")}>
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="login">Login</TabsTrigger>
            <TabsTrigger value="signup">Sign Up</TabsTrigger>
          </TabsList>
          <TabsContent value="login" className="mt-6">
            <LoginForm />
          </TabsContent>
          <TabsContent value="signup" className="mt-6">
            <SignUpForm />
          </TabsContent>
        </Tabs>
      </CardContent>
    </Card>
  );
}
