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
import { commonTranslations } from "../../../translations/common";
import { authTranslations } from "../../../translations/pages/authentication";
import { emailRegExp } from "../../../util/input-validation";
import {
  ForgotPasswordContextProvider,
  useForgotPasswordContext,
} from "./context/forgot-password-context";

export type ForgotPasswordFormData = {
  email: string;
};

export const ForgotPasswordPage = () => {
  return (
    <ForgotPasswordContextProvider>
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
          <ForgotPasswordBox />
        </Backdrop>
      </Box>
    </ForgotPasswordContextProvider>
  );
};

const ForgotPasswordBox = () => {
  const { forgotPassword, loading, error } = useForgotPasswordContext();
  const navigate = useNavigate();

  const { formState, handleSubmit, control } = useForm<ForgotPasswordFormData>({
    defaultValues: {
      email: "",
    },
  });

  const handleForgotPassword: SubmitHandler<ForgotPasswordFormData> = (
    data,
  ) => {
    forgotPassword({ email: data.email });
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLDivElement>) => {
    if (event.key === "Enter") {
      handleSubmit(handleForgotPassword)();
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
      <Typography variant="h5">
        {authTranslations.forgotPasswordPage.title}
      </Typography>
      <Box height="6px" />
      <Typography variant="body1" align="center">
        {authTranslations.forgotPasswordPage.subtitle}
      </Typography>
      <Box height="30px" />
      <FormControl>
        <Controller
          name="email"
          control={control}
          rules={{
            required: authTranslations.forgotPasswordPage.emailRequired,

            pattern: {
              value: emailRegExp,
              message: authTranslations.forgotPasswordPage.emailFormat,
            },
          }}
          render={({ field }) => (
            <TextField
              {...field}
              label={authTranslations.forgotPasswordPage.email}
              variant="outlined"
              onKeyDown={handleKeyDown}
            />
          )}
        />

        <FormHelperText
          error={!!formState.errors.email}
          role={formState.errors.email ? "alert" : undefined}
        >
          {formState.errors.email?.message ?? ""}
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
        <Button onClick={() => navigate("/login")}>
          {commonTranslations.cancel}
        </Button>
        <LoadingButton
          loading={loading}
          sx={{ height: "32px" }}
          variant="contained"
          onClick={handleSubmit(handleForgotPassword)}
        >
          {authTranslations.forgotPasswordPage.confirm}
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
