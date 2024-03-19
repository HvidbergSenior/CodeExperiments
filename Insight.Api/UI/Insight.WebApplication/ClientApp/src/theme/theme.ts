import { createTheme } from "@mui/material/styles";
import { palette } from "./biofuel/palette";
import { typography } from "./typography";

export const theme = createTheme({
  palette,
  typography,
  spacing: 4,
  components: {
    MuiPaper: {
      defaultProps: {
        elevation: 0,
      },
      styleOverrides: {
        root: ({ theme }) => ({
          borderRadius: "10px",
          backgroundColor: theme.palette.Gray1.main,
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
    MuiPopover: {
      styleOverrides: {
        paper: ({ theme }) => ({
          borderRadius: theme.spacing(0.5),
        }),
      },
    },
    MuiDialog: {
      defaultProps: {
        maxWidth: "sm",
        fullWidth: true,
        slotProps: {
          backdrop: {
            style: { backgroundColor: "rgba(255, 255, 255, 0.60)" },
          },
        },
      },
      styleOverrides: {
        root: ({ theme }) => ({
          "&.MuiDialog-paper": {
            borderColor: theme.palette.common.black,
            border: "1px",
          },
        }),
      },
    },
    MuiDialogTitle: {
      styleOverrides: {
        root: ({ theme }) => ({
          paddingTop: theme.spacing(8),
          paddingRight: theme.spacing(8),
          paddingBottom: theme.spacing(8),
          paddingLeft: theme.spacing(8),
        }),
      },
    },
    MuiDialogContent: {
      styleOverrides: {
        root: ({ theme }) => ({
          paddingTop: theme.spacing(2),
          paddingRight: theme.spacing(8),
          paddingBottom: theme.spacing(1),
          paddingLeft: theme.spacing(8),
        }),
      },
    },
    MuiDialogActions: {
      styleOverrides: {
        root: ({ theme }) => ({
          display: "flex",
          justifyContent: "end",
          backgroundColor: theme.palette.background.default,
        }),
        spacing: ({ theme }) => ({
          paddingTop: theme.spacing(4),
          paddingRight: theme.spacing(8),
          paddingBottom: theme.spacing(8),
          paddingLeft: theme.spacing(4),
        }),
      },
    },
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
          ...(ownerState.variant === "outlined" && {
            border: "0px solid #c9c9c9",

            "&:hover": {
              backgroundColor: theme.palette.primary.light,
              color: theme.palette.common.white,
              boxShadow: "0px 4px 8px 0px rgba(0, 0, 0, .3)",
              border: "0px solid #c9c9c9",
            },
          }),
          textWrap: "nowrap",
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
        }),
      },
    },
    MuiIconButton: {
      styleOverrides: {
        root: ({ theme }) => ({
          borderRadius: theme.spacing(1),
        }),
      },
    },
    MuiButtonGroup: {
      styleOverrides: {
        root: () => ({
          backgroundColor: "transparent",
          boxShadow: "none",
        }),
      },
    },
    MuiCheckbox: {
      styleOverrides: {
        root: ({ theme }) => ({
          color: theme.palette.primary.main,
        }),
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          fontSize: "10px",
          lineHeight: "14px",
          fontWeight: 700,
        },
      },
    },
    MuiList: {
      styleOverrides: {
        root: {
          padding: 0,
        },
      },
    },
    MuiListItemButton: {
      styleOverrides: {
        root: ({ theme }) => ({
          "&.Mui-selected": {
            color: theme.palette.primary.main,
            backgroundColor: theme.palette.Green10.main,
          },
          "&:hover": {
            backgroundColor: theme.palette.primary.contrastText,
          },
          color: theme.palette.primary.main,
        }),
      },
    },
    MuiFormControl: {
      defaultProps: {
        fullWidth: true,
      },
      styleOverrides: {
        root: {
          minWidth: "auto",
        },
      },
    },
    MuiFormControlLabel: {
      styleOverrides: {
        root: {
          marginLeft: 0,
        },
      },
    },
    MuiFormLabel: {
      styleOverrides: {
        root: ({ theme }) => ({
          color: "#787874",
          fontSize: "12px",
          fontWeight: 500,
          paddingBottom: theme.spacing(1),
        }),
      },
    },
    MuiAutocomplete: {
      styleOverrides: {
        paper: ({ theme }) => ({
          backgroundColor: theme.palette.primary.main,
          marginTop: "5px",
          borderRadius: "4px",
          boxShadow:
            "0px 2px 1px -1px rgb(0 0 0 / 20%), 0px 1px 1px 0px rgb(0 0 0 / 14%), 0px 1px 3px 0px rgb(0 0 0 / 12%)",
        }),
      },
    },

    MuiFilledInput: {
      styleOverrides: {
        root: ({ theme }) => ({
          backgroundColor: theme.palette.primary.main,
          borderRadius: "4px",
          "& .MuiInputBase-root": {
            borderRadius: "4px",
          },
        }),
        error: ({ theme }) => ({
          backgroundColor: "#FFF3F8",
          border: `1px solid ${theme.palette.primary.main}`,
        }),
      },
    },
    MuiOutlinedInput: {
      styleOverrides: {
        root: ({ theme }) => ({
          background: theme.palette.common.white,
        }),
      },
    },
    MuiInputLabel: {
      styleOverrides: {
        root: {
          fontSize: "16px",
        },
      },
    },
    MuiInputBase: {
      styleOverrides: {
        root: {
          fontSize: "16px",
        },
      },
    },
    MuiTab: {
      defaultProps: {
        disableRipple: true,
      },
      styleOverrides: {
        root: ({ theme }) => ({
          "&.Mui-selected": {
            color: theme.palette.common.black,
          },
          textTransform: "none",
          fontSize: "15px",
          fontWeight: "600",
          color: theme.palette.common.black,
        }),
      },
    },
    MuiSelect: {
      defaultProps: {
        variant: "outlined",
        fullWidth: true,
      },
    },
    MuiTableHead: {
      styleOverrides: {
        root: ({ theme }) => ({
          "& .MuiTableCell-root": {
            background: theme.palette.Green2.main,
            alignContent: "center",
            display: "flex",
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
    MuiLink: {
      styleOverrides: {
        root: ({ theme }) => ({
          color: theme.palette.primary.main,
        }),
      },
    },
    MuiMenuItem: {
      styleOverrides: {
        root: ({ theme }) => ({
          background: theme.palette.common.white,
          fontSize: "16px",
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
