import { faInfoCircle, faWarning, IconDefinition } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useEffect, useState } from "react";
import { MessageType } from "../../helpers/enums/MessageTypeEnum";

interface InfoTextProps {
  messageType: MessageType;
  text: string;
  isSemibold?: boolean;
  fontSize?: number;
  iconSize?: number;
  maxWidth?: number;
}

const MessageText = ({
  messageType,
  text,
  isSemibold = false,
  fontSize = 16,
  iconSize = 16,
  maxWidth,
}: InfoTextProps) => {
  const [textColor, setTextColor] = useState("#0ea5e9");
  const [icon, setIcon] = useState<IconDefinition>(faInfoCircle);

  useEffect(() => {
    switch (messageType) {
      case MessageType.Error:
        setTextColor("#ef4444");
        setIcon(faWarning);
        break;
      case MessageType.Info:
        setTextColor("#0ea5e9");
        setIcon(faInfoCircle);
        break;
    }
  }, [messageType]);

  return (
    <p style={{ color: textColor, fontWeight: `${isSemibold ? 600 : 400}`, maxWidth: maxWidth }}>
      <span>
        <FontAwesomeIcon icon={icon} style={{ color: textColor, fontSize: iconSize }} />
      </span>
      &nbsp; <span style={{ fontSize: fontSize }}>{text}</span>
    </p>
  );
};
export default MessageText;
