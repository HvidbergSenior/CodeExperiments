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
import {
  ChangePasswordContextProvider,
  useChangePasswordContext,
} from "./context/change-password-context";

export type ChangePasswordFormData = {
  password: string;
  confirmPassword: string;
};

export const ChangePasswordPage = () => {
  return (
    <ChangePasswordContextProvider>
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
          <ChangePasswordBox />
        </Backdrop>
      </Box>
    </ChangePasswordContextProvider>
  );
};

const ChangePasswordBox = () => {
  const { changePassword, loading, error } = useChangePasswordContext();
  const navigate = useNavigate();

  const { formState, handleSubmit, control, getValues } =
    useForm<ChangePasswordFormData>({
      defaultValues: {
        password: "",
        confirmPassword: "",
      },
    });

  const handleChangePassword: SubmitHandler<ChangePasswordFormData> = (
    data,
  ) => {
    changePassword({ password: data.password });
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLDivElement>) => {
    if (event.key === "Enter") {
      handleSubmit(handleChangePassword)();
    }
  };

  return (
    <Box
      width="600px"
      height="500px"
      display="flex"
      padding={(theme) => theme.spacing(14)}
      pb={6}
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
        {authTranslations.changePasswordPage.title}
      </Typography>
      <Box height="6px" />
      <Typography variant="body1" align="center">
        {authTranslations.changePasswordPage.subtitle}
      </Typography>

      <Box height="30px" />

      <FormControl>
        <Controller
          name="password"
          control={control}
          rules={{
            required: authTranslations.changePasswordPage.passwordRequired,
          }}
          render={({ field }) => (
            <TextField
              {...field}
              label={authTranslations.changePasswordPage.password}
              variant="outlined"
              type="password"
              onKeyDown={handleKeyDown}
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

      <Box height="1rem" />

      <FormControl>
        <Controller
          name="confirmPassword"
          control={control}
          rules={{
            required:
              authTranslations.changePasswordPage.confirmPasswordRequired,
            validate: {
              equalToOther: (v) => {
                if (v !== getValues("password")) {
                  return authTranslations.changePasswordPage
                    .confirmPasswordMustBeEqual;
                }
              },
            },
          }}
          render={({ field }) => (
            <TextField
              {...field}
              label={authTranslations.changePasswordPage.confirmPassword}
              variant="outlined"
              type="password"
              onKeyDown={handleKeyDown}
            />
          )}
        />

        <FormHelperText
          error={!!formState.errors.confirmPassword}
          role={formState.errors.confirmPassword ? "alert" : undefined}
        >
          {formState.errors.confirmPassword?.message ?? ""}
        </FormHelperText>
      </FormControl>
      <Typography mt={2} variant="subtitle1" align="left">
        {authTranslations.changePasswordPage.passwordRequirements}
      </Typography>
      <Box
        display="flex"
        width="100%"
        flexDirection="row"
        justifyContent="end"
        alignItems="center"
        mt={10}
        gap={4}
      >
        <Button onClick={() => navigate("/login")}>
          {commonTranslations.cancel}
        </Button>
        <LoadingButton
          loading={loading}
          sx={{ height: "32px" }}
          variant="contained"
          onClick={handleSubmit(handleChangePassword)}
        >
          {authTranslations.changePasswordPage.confirm}
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
