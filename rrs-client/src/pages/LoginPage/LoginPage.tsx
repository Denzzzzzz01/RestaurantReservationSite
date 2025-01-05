import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from 'react-hook-form';
import { Link } from "react-router-dom";
import { useAuth } from "../../context/useAuth";
import './LoginPage.scss';

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
    <section className="login-page">
      <div className="login-container">
        <div className="login-header">
          <div className="header-image">
            <img src="/images/enter.png" alt="Enter" />
          </div>
          <h1>Sign in to your account</h1>
        </div>

        <form className="login-form" onSubmit={handleSubmit(handleLogin)}>
          <div className="form-group">
            <i className="fi fi-rr-envelope input-icon"></i>
            <input
              type="text"
              id="email"
              placeholder="Email"
              {...register("email")}
            />
            {errors.email && <p className="error-message">{errors.email.message}</p>}
          </div>

          <div className="form-group">
            <i className="fi fi-rr-lock input-icon"></i>
            <input
              type="password"
              id="password"
              placeholder="Password"
              {...register("password")}
            />
            {errors.password && <p className="error-message">{errors.password.message}</p>}
          </div>

          <button type="submit" className="login-button">Sign In</button>
        </form>

        <div className="register-link">
          <p>
            Don't have an account? <a href="/register">Register</a>
          </p>
        </div>
      </div>
    </section>

  );
}

export default LoginPage;
