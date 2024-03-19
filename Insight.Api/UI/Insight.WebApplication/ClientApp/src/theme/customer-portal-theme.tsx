import { createTheme } from "@mui/material/styles";
import { palette } from "./biofuel/palette";
import { typography } from "./typography";
import { theme } from "./theme";

export const customerPortalTheme = createTheme({
  palette,
  typography,
  spacing: 4,
  components: {
    ...theme.components,
    MuiButton: {
      defaultProps: {
        disableElevation: true,
      },
      styleOverrides: {
        root: ({ ownerState, theme }) => ({
          "&.MuiButtonGroup-grouped": {
            backgroundColor: "transparent",
            fontSize: "18px",
            boxShadow: "none",
            color: theme.palette.primary.main,
            paddingTop: theme.spacing(1),
            paddingRight: theme.spacing(6),
            paddingBottom: theme.spacing(1),
            paddingLeft: theme.spacing(6),
            "&:hover": {
              backgroundColor: theme.palette.primary.contrastText,
              color: theme.palette.primary.main,
            },
            "&:focus": {
              backgroundColor: "transparent",
              color: theme.palette.primary.main,
            },
          },

          "&:hover": {
            backgroundColor: theme.palette.primary.light,
            color: theme.palette.common.white,
          },
          "&:focus": {
            backgroundColor: theme.palette.primary.main,
            color: theme.palette.common.white,
          },
          backgroundColor: theme.palette.common.white,
          color: theme.palette.common.black,
          textTransform: "capitalize",
          minWidth: theme.spacing(18),
          minHeight: theme.spacing(5),
          paddingTop: theme.spacing(1),
          paddingRight: theme.spacing(3),
          paddingBottom: theme.spacing(1),
          paddingLeft: theme.spacing(3),
          boxShadow:
            "0px 3px 1px -2px rgba(0, 0, 0, 0.20), 0px 2px 2px 0px rgba(0, 0, 0, 0.14), 0px 1px 5px 0px rgba(0, 0, 0, 0.12)",
          borderRadius: "4px",
          ...(ownerState.variant === "text" && {
            boxShadow: "none",
            "&:hover": {
              backgroundColor:
                ownerState.color !== undefined && ownerState.color !== "inherit"
                  ? theme.palette[ownerState.color].contrastText
                  : "inherit",
            },
          }),
          ...(ownerState.variant === "contained" &&
            ownerState.color === "primary" && {
              textTransform: "none",
              backgroundColor: "#28903a",
              color: theme.palette.common.white,
              borderRadius: "100px",
              fontWeight: "600",
              paddingRight: theme.spacing(7),
              paddingLeft: theme.spacing(7),
              boxShadow: "none",
              height: "48px",

              "&:hover": {
                backgroundColor: "#247a33",
                boxShadow: "0px 4px 8px 0px rgba(0, 0, 0, .3)",
              },
            }),
          ...(ownerState.variant === "outlined" && {
            textTransform: "none",
            borderRadius: "100px",
            fontWeight: "bold",
            paddingRight: theme.spacing(7),
            paddingLeft: theme.spacing(7),
            boxShadow: "none",
            border: "2px solid #c9c9c9",
            height: "48px",
            "&:hover": {
              color: "#8cc63f",
              backgroundColor: "#f4f4f4",
              boxShadow: "0px 4px 8px 0px rgba(0, 0, 0, .3)",
              border: "2px solid #c9c9c9",
            },
          }),
        }),
      },
    },
    MuiPaper: {
      defaultProps: {
        elevation: 0,
      },
      styleOverrides: {
        root: ({ theme }) => ({
          borderRadius: "10px",
          backgroundColor: theme.palette.common.white,
          "&.MuiDialog-paper": {
            backgroundColor: theme.palette.common.white,
            borderColor: theme.palette.common.white,
            border: `1px solid ${theme.palette.common.black}`,
          },
          "&.MuiTableContainer-root": {
            backgroundColor: theme.palette.common.white,
            borderRadius: "0px",
          },
          "& .MuiPickersDay-root": {
            "&.Mui-selected": {
              backgroundColor: "rgba(0, 122 , 51 , 0.10)",
              color: theme.palette.primary.main,
              "&:focus": {
                backgroundColor: "rgba(0, 122 , 51 , 0.10)",
                color: theme.palette.primary.main,
              },
            },
          },
          "& .MuiPickersYear-yearButton": {
            "&.Mui-selected": {
              backgroundColor: "rgba(0, 122 , 51 , 0.10)",
              color: theme.palette.primary.main,
              "&:focus": {
                backgroundColor: "rgba(0, 122 , 51 , 0.10)",
                color: theme.palette.primary.main,
              },
            },
          },
        }),
      },
    },
    MuiTableHead: {
      styleOverrides: {
        root: ({ theme }) => ({
          "& .MuiTableCell-root": {
            background: theme.palette.common.white,
            alignContent: "center",
            alignItems: "center",
            display: "flex",
            borderBottom: "2px solid black",
          },
          "&:hover": {
            "& .MuiTableCell-root": {
              background: theme.palette.Green2.main,
            },
          },
          "& .MuiTableRow-root th:first-of-type": {
            borderTopLeftRadius: "10px",
          },
          "& .MuiTableRow-root th:last-child": {
            borderTopRightRadius: "10px",
          },
        }),
      },
    },
    MuiTableBody: {
      styleOverrides: {
        root: {},
      },
    },

    MuiTableCell: {
      styleOverrides: {
        root: ({ theme }) => ({
          "&:hover": {
            backgroundColor: theme.palette.Green1.main,
          },
        }),
      },
    },

    MuiTableRow: {
      styleOverrides: {
        root: ({ theme }) => ({
          "&:hover": {
            "& .MuiTableCell-root": {
              backgroundColor: theme.palette.primary.contrastText,
            },
          },
          "&.Mui-selected": {
            "& .MuiTableCell-root": {
              backgroundColor: theme.palette.primary.contrastText,
            },
          },
          background: theme.palette.common.white,
          "& td:first-of-type, & th:first-of-type": {
            paddingLeft: "30px",
          },
          "& td:last-of-type, & th:last-of-type": {
            paddingRight: "30px",
          },
        }),
      },
    },
    MuiSelect: {
      styleOverrides: {
        root: () => ({
          "& .MuiInputBase-input": {
            padding: "7px 5px 7px 16px",
            fontSize: "14px",
            height: "10px",
          },
        }),
      },
    },
    MuiFormControlLabel: {
      styleOverrides: {
        root: () => ({
          marginTop: "-9px",
          fontSize: "16px",
        }),
      },
    },
    MuiFormControl: {
      styleOverrides: {
        root: () => ({
          "& .MuiFormLabel-root": {
            marginTop: "-7px",
            fontSize: "14px",
          },
        }),
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: () => ({
          "& .MuiFormLabel-root": {
            marginTop: "0px",
            fontSize: "14px",
          },
        }),
      },
    },
    MuiMenuItem: {
      styleOverrides: {
        root: ({ theme }) => ({
          background: theme.palette.common.white,
          fontSize: "14px",
          lineHeight: "24px",
          letterSpacing: "0.15px",
          "&:hover": {
            background: theme.palette.Green13.main,
          },
          ":focus": {
            background: theme.palette.Green13.light,
          },
          "&.Mui-disabled": {
            background: "theme.palette.common.red",
          },
        }),
      },
    },
  },
});
