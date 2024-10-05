import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from 'react-hook-form';
import { Link } from "react-router-dom";
import { useAuth } from "../../context/useAuth";

type Props = {};

type LoginFormsInputs = {
  email: string;
  password: string;
};

const validation = Yup.object().shape({
  email: Yup.string().required("Email is required").email("Invalid email address"),
  password: Yup.string().required("Password is required"),
});

const LoginPage = (props: Props) => {
  const { loginUser } = useAuth();
  const { register, handleSubmit, formState: { errors } } = useForm<LoginFormsInputs>({ resolver: yupResolver(validation) });

  const handleLogin = (form: LoginFormsInputs) => {
    loginUser(form.email, form.password);
  };

  return (
    <section className="flex flex-col items-center justify-center w-full h-full rounded-b-lg max-w-md">
      <div className="rounded-lg shadow-lg max-w-md w-full bg-black bg-opacity-25">

        <div className="bg-white pt-8 pl-8 pr-8 pb-4 rounded-t-lg rounded-r-lg shadow-lg max-w-md w-full">
          <h1 className="text-2xl font-bold mb-6 text-center">Sign in to your account</h1>
          <form onSubmit={handleSubmit(handleLogin)} className="">
            <div className="mt-4">
              <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
              <input
                type="text"
                id="email"
                placeholder="Email"
                {...register("email")}
                className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-beige focus:border-beige sm:text-sm"
              />
              {errors.email && <p className="mt-2 text-sm text-red-600">{errors.email.message}</p>}
            </div>
            <div className="mt-4 ">
              <label htmlFor="password" className="block text-sm font-medium text-gray-700">Password</label>
              <input
                type="password"
                id="password"
                placeholder="••••••••"
                {...register("password")}
                className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-beige focus:border-beige sm:text-sm"
              />
              {errors.password && <p className="mt-2 text-sm text-red-600">{errors.password.message}</p>}
            </div>
            <button 
              type="submit" 
              className="w-full flex justify-center mt-10 py-3 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-beige hover:bg-beige-dark focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-beige-light ">
                Sign in
            </button>
          </form>
        </div>

        <div className=' max-w-md w-full rounded-b-lg flex items-center justify-around'>
          <Link to="/login" className="w-full rounded-b-lg p-3 text-center bg-white ">
            SignIn
          </Link>
          <Link to="/register" className="w-full p-3 text-center rounded-b-lg bg-black bg-opacity-0 hover:bg-opacity-20 text-gray-300 ">
            SignUp
          </Link>
        </div>
      
      </div>
    </section>
  );
}

export default LoginPage;
