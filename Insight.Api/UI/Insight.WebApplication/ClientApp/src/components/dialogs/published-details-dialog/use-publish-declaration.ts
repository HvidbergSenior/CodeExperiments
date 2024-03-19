import React from "react";
import { authorizedHttpClient } from "../../../api";
import { OutgoingDeclarationByIdResponse } from "../../../api/api";
import useHandleNetworkError from "../../../util/errors/use-handle-network-error";

interface Props {
  declarationId: string;
}
export const usePublishDeclaration = ({ declarationId }: Props) => {
  const [loading, setLoading] = React.useState(true);
  const [declaration, setDeclaration] = React.useState<
    OutgoingDeclarationByIdResponse | undefined
  >(undefined);
  const { showErrorDialog } = useHandleNetworkError();

  const getDeclaration = async () => {
    try {
      const response =
        await authorizedHttpClient.api.getOutgoingDeclarationById(
          declarationId,
        );

      setDeclaration(response.data.outgoingDeclarationByIdResponse);
    } catch (error) {
      showErrorDialog(error);
    } finally {
      setLoading(false);
    }
  };

  React.useEffect(() => {
    getDeclaration();
  }, []);

  return { declaration, loading };
};
