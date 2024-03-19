import React from "react";
import { authorizedHttpClient } from "../../api";
import { CustomerPortalFilter } from "../../pages/customer-portal/customer-portal-context";
import useHandleNetworkError from "../../util/errors/use-handle-network-error";
import { addParamsToUrl } from "../../util/url/url-params";
import { useAuthContext } from "../../pages/authentication/login/context/auth-context";

export function downloadContentAsFile(
  content: any,
  filename: string,
  contentType: string,
) {
  if (!contentType) contentType = "application/octet-stream";
  var a = document.createElement("a");
  var blob = new Blob([content], { type: contentType });
  a.href = window.URL.createObjectURL(blob);
  a.download = filename;
  a.click();
}

interface Props {
  setLoading: (value: React.SetStateAction<boolean>) => void;
  route: string;
  fileName: string;
  filter: CustomerPortalFilter;
}
export const usePdfDownload = ({
  fileName,
  route,
  setLoading,
  filter,
}: Props) => {
  const { showErrorDialog } = useHandleNetworkError();
  const { username } = useAuthContext();

  const handleDownload = async () => {
    setLoading(true);
    const url = "https://" + window.location.host + `/${route}`;
    const urlWithParams = addParamsToUrl(url, { ...filter, username });
    try {
      const response = await authorizedHttpClient.api.generatepdf(
        {
          targetUrl: encodeURI(urlWithParams),
        },
        {
          format: "blob",
        },
      );
      downloadContentAsFile(response.data, fileName, "application/pdf");
    } catch (error) {
      showErrorDialog(error);
    } finally {
      setLoading(false);
    }
  };

  return { handleDownload };
};
