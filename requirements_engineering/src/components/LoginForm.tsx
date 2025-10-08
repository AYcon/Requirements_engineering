import { useState } from "react";
import { Button } from "./ui/button";
import { Input } from "./ui/input";
import { Label } from "./ui/label";
import { Separator } from "./ui/separator";
import { Checkbox } from "./ui/checkbox";
import { toast } from "sonner";
import { Mail, Lock, Github } from "lucide-react";

export function LoginForm() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!email || !password) {
      toast.error("Please fill in all fields");
      return;
    }

    try {
      setLoading(true);

      const response = await fetch("https://localhost:44386/api/users/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (response.status === 401) {
        toast.error("Invalid email or password");
        return;
      }

      if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Login failed");
      }

      const user = await response.json();
      toast.success(`Welcome back, ${user.name}!`);
      console.log("User logged in:", user);

      // optionally persist user info (if rememberMe checked)
      if (rememberMe) {
        localStorage.setItem("user", JSON.stringify(user));
      }

      // reset form
      setEmail("");
      setPassword("");
      setRememberMe(false);
    } catch (err: any) {
      toast.error(err.message || "Something went wrong");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      {/* Email */}
      <div className="space-y-2">
        <Label htmlFor="login-email">Email</Label>
        <div className="relative">
          <Mail className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            id="login-email"
            type="email"
            placeholder="you@example.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="pl-10"
            required
          />
        </div>
      </div>

      {/* Password */}
      <div className="space-y-2">
        <div className="flex items-center justify-between">
          <Label htmlFor="login-password">Password</Label>
          <button
            type="button"
            className="text-muted-foreground hover:text-foreground transition-colors"
            onClick={() => toast.info("Password reset link sent to your email")}
          >
            Forgot?
          </button>
        </div>
        <div className="relative">
          <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            id="login-password"
            type="password"
            placeholder="••••••••"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="pl-10"
            required
          />
        </div>
      </div>

      {/* Remember Me */}
      <div className="flex items-center space-x-2">
        <Checkbox
          id="remember"
          checked={rememberMe}
          onCheckedChange={(checked: boolean | "indeterminate") =>
            setRememberMe(checked === true)
          }
        />
        <label
          htmlFor="remember"
          className="text-muted-foreground cursor-pointer"
        >
          Remember me
        </label>
      </div>

      {/* Submit */}
      <Button type="submit" className="w-full" disabled={loading}>
        {loading ? "Signing in..." : "Sign In"}
      </Button>

      {/* Divider */}
      <div className="relative my-4">
        <Separator />
        <span className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 bg-card px-2 text-muted-foreground">
          or continue with
        </span>
      </div>

      {/* Social login (mock) */}
      <div className="grid grid-cols-2 gap-3">
        <Button
          type="button"
          variant="outline"
          onClick={() => toast.info("Google sign in not implemented")}
        >
          <svg className="h-4 w-4 mr-2" viewBox="0 0 24 24">
            <path
              fill="currentColor"
              d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"
            />
            <path
              fill="currentColor"
              d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"
            />
            <path
              fill="currentColor"
              d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"
            />
            <path
              fill="currentColor"
              d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"
            />
          </svg>
          Google
        </Button>
        <Button
          type="button"
          variant="outline"
          onClick={() => toast.info("GitHub sign in not implemented")}
        >
          <Github className="h-4 w-4 mr-2" />
          GitHub
        </Button>
      </div>
    </form>
  );
}
