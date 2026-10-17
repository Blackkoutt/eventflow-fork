import { forwardRef, useState } from "react";
import Dialog from "../../common/Dialog";
import { EventEntity } from "../../../models/response_models";
import EventDetails from "./EventDetails";
import { EventTabButtonType } from "../../../helpers/enums/EventTabButtonType";
import EventTabButton from "../../buttons/EventTabButton";
import ModifyEvent from "./ModifyEvent";
import ModifyEventHall from "./ModifyEventHall";
import DownloadEventHall from "./DownloadEventHall";
import DeleteEvent from "./DeleteEvent";

interface EventClickDialogProps {
  item?: EventEntity;
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

const EventClickDialog = forwardRef<HTMLDialogElement, EventClickDialogProps>(
  (
    { item, maxWidth, minWidth, onDialogClose, paddingX, onDialogSuccess }: EventClickDialogProps,
    ref
  ) => {
    const [currentTab, setCurrentTab] = useState<string>(EventTabButtonType.Details);

    return (
      <Dialog
        ref={ref}
        top={250}
        maxWidth={maxWidth}
        paddingLeft={paddingX}
        paddingRight={paddingX}
        minWidth={minWidth}
        onClose={onDialogClose}
      >
        <div className="flex flex-col justify-center items-center w-full gap-5">
          <div className="flex flex-row justify-end items-center gap-4 w-full mt-2">
            <EventTabButton
              currentTab={currentTab}
              tabString={EventTabButtonType.Details}
              text="SZCZEGÓŁY"
              onClick={() => {
                setCurrentTab(EventTabButtonType.Details);
              }}
            />
            <EventTabButton
              currentTab={currentTab}
              tabString={EventTabButtonType.Modify}
              text="MODYFIKUJ"
              onClick={() => {
                setCurrentTab(EventTabButtonType.Modify);
              }}
            />
            <EventTabButton
              currentTab={currentTab}
              tabString={EventTabButtonType.ModifyHall}
              text="MODYFIKUJ SALE"
              onClick={() => {
                setCurrentTab(EventTabButtonType.ModifyHall);
              }}
            />
            <EventTabButton
              currentTab={currentTab}
              tabString={EventTabButtonType.Download}
              text="POBIERZ"
              onClick={() => {
                setCurrentTab(EventTabButtonType.Download);
              }}
            />
            <EventTabButton
              currentTab={currentTab}
              tabString={EventTabButtonType.Delete}
              text="USUŃ"
              onClick={() => {
                setCurrentTab(EventTabButtonType.Delete);
              }}
            />
          </div>
          <div className="w-full">
            {currentTab === EventTabButtonType.Details && <EventDetails item={item} />}
            {currentTab === EventTabButtonType.Modify && (
              <ModifyEvent
                onDialogSuccess={onDialogSuccess}
                onDialogClose={onDialogClose}
                item={item}
              />
            )}
            {currentTab === EventTabButtonType.ModifyHall && (
              <ModifyEventHall
                item={item}
                onDialogSuccess={onDialogSuccess}
                onDialogClose={onDialogClose}
              />
            )}
            {currentTab === EventTabButtonType.Download && (
              <DownloadEventHall onDialogClose={onDialogClose} item={item} />
            )}
            {currentTab === EventTabButtonType.Delete && (
              <DeleteEvent
                item={item}
                onDialogSuccess={onDialogSuccess}
                onDialogClose={onDialogClose}
              />
            )}
          </div>
        </div>
      </Dialog>
    );
  }
);
export default EventClickDialog;
