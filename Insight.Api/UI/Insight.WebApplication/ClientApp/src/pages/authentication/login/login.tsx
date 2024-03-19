import {
  Backdrop,
  Box,
  Button,
  FormControl,
  FormHelperText,
  TextField,
  Typography,
} from "@mui/material";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { LoadingButton } from "../../../components/loading-button.tsx/loading-button";
import { authTranslations } from "../../../translations/pages/authentication";
import { LoginFormData } from "../../types";
import { useAuthContext } from "./context/auth-context";
import { AnimationEvent, useEffect, useState } from "react";

export const Login = () => {
  return (
    <Box
      display="flex"
      width="100%"
      height="100vh"
      sx={{
        background: "url(/assets/background.png)",
        backgroundSize: "cover",
      }}
      justifyContent="center"
      alignItems="center"
    >
      <Backdrop
        sx={{
          backgroundColor: (theme) => theme.palette.TransparentWhite.main,
          zIndex: 2,
        }}
        open={true}
      >
        <LoginBox />
      </Backdrop>
    </Box>
  );
};

const LoginBox = () => {
  const { login, loading, error } = useAuthContext();
  const navigate = useNavigate();
  const [usernameHasAutofill, setUsernameHasAutofill] = useState(false);
  const [passwordHasAutofill, setPasswordHasAutofill] = useState(false);

  const { formState, handleSubmit, control, watch } = useForm<LoginFormData>({
    defaultValues: {
      userName: "",
      password: "",
    },
  });

  const { userName, password } = watch();

  useEffect(() => {
    setUsernameHasAutofill(userName !== "");
    setPasswordHasAutofill(password !== "");
  }, [userName, password]);

  const handleLogin: SubmitHandler<LoginFormData> = (data) => {
    login({
      username: data.userName,
      password: data.password,
    });
  };

  const handlePasswordKeyDown = (
    event: React.KeyboardEvent<HTMLDivElement>,
  ) => {
    if (event.key === "Enter") {
      handleSubmit(handleLogin)();
    }
  };

  const makeAnimationStartHandler =
    (stateSetter: React.Dispatch<React.SetStateAction<boolean>>) =>
    (e: AnimationEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      // checks if textfield has been auto filled and removes placeholder in textfield if necessary
      const autofilled = !!e.currentTarget.matches("*:-webkit-autofill");
      if (e.animationName === "mui-auto-fill") {
        stateSetter(autofilled);
        return;
      }

      if (e.animationName === "mui-auto-fill-cancel") {
        stateSetter(autofilled);
        return;
      }
    };

  return (
    <Box
      width="600px"
      height="500px"
      display="flex"
      padding={(theme) => theme.spacing(14)}
      flexDirection="column"
      alignItems="center"
      justifyContent="center"
      sx={{
        backgroundColor: (theme) => theme.palette.common.white,
        borderRadius: "5px",
        boxShadow: "0 3px 10px rgb(0 0 0 / 0.6)",
      }}
    >
      <Box>
        <img width="100%" src={"/assets/logo.png"} loading="lazy" />
      </Box>
      <Box height="50px" />
      <Typography variant="h5">{authTranslations.subtitle}</Typography>
      <Box height="1rem" />
      <FormControl>
        <Controller
          name="userName"
          control={control}
          rules={{ required: authTranslations.userNameRequired }}
          render={({ field }) => (
            <TextField
              {...field}
              label={authTranslations.userName}
              variant="outlined"
              onKeyDown={handlePasswordKeyDown}
              inputProps={{
                onAnimationStart: makeAnimationStartHandler(
                  setUsernameHasAutofill,
                ),
              }}
              InputLabelProps={{ shrink: usernameHasAutofill }}
            />
          )}
        />

        <FormHelperText
          error={!!formState.errors.userName}
          role={formState.errors.userName ? "alert" : undefined}
        >
          {formState.errors.userName?.message ?? ""}
        </FormHelperText>
      </FormControl>
      <Box height="1rem" />
      <FormControl>
        <Controller
          name="password"
          control={control}
          rules={{ required: authTranslations.passwordRequired }}
          render={({ field }) => (
            <TextField
              {...field}
              label={authTranslations.password}
              variant="outlined"
              type="password"
              onKeyDown={handlePasswordKeyDown}
              inputProps={{
                onAnimationStart: makeAnimationStartHandler(
                  setPasswordHasAutofill,
                ),
              }}
              InputLabelProps={{ shrink: passwordHasAutofill }}
            />
          )}
        />

        <FormHelperText
          error={!!formState.errors.password}
          role={formState.errors.password ? "alert" : undefined}
        >
          {formState.errors.password?.message ?? ""}
        </FormHelperText>
      </FormControl>
      <Box
        display="flex"
        width="100%"
        flexDirection="row"
        justifyContent="end"
        alignItems="center"
        mt={4}
        gap={4}
      >
        <Button variant="text" onClick={() => navigate("/forgot-password")}>
          {authTranslations.forgotPassword}
        </Button>
        <LoadingButton
          loading={loading}
          sx={{ height: "32px" }}
          variant="contained"
          onClick={handleSubmit(handleLogin)}
        >
          {authTranslations.login}
        </LoadingButton>
      </Box>
      <Box
        display="flex"
        width="100%"
        flexDirection="row"
        justifyContent="end"
        alignItems="center"
        mt={4}
      >
        {error && (
          <Typography mr={4} variant="error">
            {error.detail}
          </Typography>
        )}
      </Box>
    </Box>
  );
};
