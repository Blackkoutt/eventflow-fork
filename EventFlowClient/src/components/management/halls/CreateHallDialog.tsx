import { forwardRef, useEffect, useState } from "react";
import Dialog from "../../common/Dialog";
import { Hall, HallType, Seat, SeatType } from "../../../models/response_models";
import Button, { ButtonStyle } from "../../buttons/Button";
import useApi from "../../../hooks/useApi";
import { ApiEndpoint } from "../../../helpers/enums/ApiEndpointEnum";
import { HTTPStatusCode } from "../../../helpers/enums/HTTPStatusCode";
import { faCheck, faCouch, faXmark } from "@fortawesome/free-solid-svg-icons";
import { FormProvider, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import FormButton from "../../common/forms/FormButton";
import Input from "../../common/forms/Input";
import Select from "../../common/forms/Select";
import { SelectOption } from "../../../helpers/SelectOptions";
import { HallRequest, HallSchema } from "../../../models/create_schemas/HallSchema";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { SeatRequest, SeatSchema } from "../../../models/create_schemas/SeatSchema";
import ContextMenu from "../../common/ContextMenu";
import { toast } from "react-toastify";

interface CreateHallDialogProps {
  maxWidth?: number;
  minWidth?: number;
  paddingX?: number;
  onDialogSuccess: () => void;
  onDialogClose: () => void;
}

interface ContextMenuState {
  selectedSeatNr?: number;
  position: { x: number; y: number };
  items: { label: string; action: (selectedSeatNr?: number) => void }[];
}

const CreateHallDialog = forwardRef<HTMLDialogElement, CreateHallDialogProps>(
  (
    { onDialogClose, onDialogSuccess }: CreateHallDialogProps,
    ref
  ) => {
    const { statusCode: statusCode, post: postItem } = useApi<Hall, HallRequest>(ApiEndpoint.Hall);
    const { data: seatTypes, get: getSeatTypes } = useApi<SeatType>(ApiEndpoint.SeatType);
    const { data: hallTypes, get: getHallTypes } = useApi<HallType>(ApiEndpoint.HallType);

    const [hallSeats, setHallSeats] = useState<Seat[]>([]);
    const [contextMenu, setContextMenu] = useState<ContextMenuState | null>(null);
    const [actionPerformed, setActionPerformed] = useState(false);
    const [, setPromisePending] = useState(false);
    const [selectOptions, setSelectOptions] = useState<SelectOption[]>([]);
    const [selectedOption] = useState<SelectOption>();
    const [currentMaxNumberRows, setCurrentMaxNumberRows] = useState(10);
    const [currentMaxNumberCols, setCurrentMaxNumberCols] = useState(10);

    const methods = useForm<HallRequest>({
      resolver: zodResolver(HallSchema),
      defaultValues: {
        maxNumberOfSeatsRows: 10,
        maxNumberOfSeatsColumns: 10,
      },
    });
    const { handleSubmit, formState, reset, watch } = methods;
    const { errors, isSubmitting } = formState;

    const maxRows = watch().maxNumberOfSeatsRows;
    const maxCols = watch().maxNumberOfSeatsColumns;

    useEffect(() => {
      if (maxRows >= 1 && maxRows <= 20) setCurrentMaxNumberRows(maxRows);
      else if (maxCols >= 20) setCurrentMaxNumberRows(20);
      else if (maxCols <= 1) setCurrentMaxNumberRows(1);
    }, [maxRows]);

    useEffect(() => {
      if (maxCols >= 1 && maxCols <= 20) setCurrentMaxNumberCols(maxCols);
      else if (maxCols >= 20) setCurrentMaxNumberCols(20);
      else if (maxCols <= 1) setCurrentMaxNumberCols(1);
    }, [maxCols]);

    useEffect(() => {
      getSeatTypes({ id: undefined, queryParams: undefined });
      getHallTypes({ id: undefined, queryParams: undefined });
      reset({
        maxNumberOfSeatsRows: 10,
        maxNumberOfSeatsColumns: 10,
      });
    }, []);

    useEffect(() => {
      const selectOptions: SelectOption[] = hallTypes.map(
        (hallType) =>
          ({
            value: hallType.id,
            option: hallType.name,
          } as SelectOption)
      );
      setSelectOptions(selectOptions);
    }, [hallTypes]);

    const changeSeatType = (
      seatNumber: number,
      seatType?: SeatType,
      maxNumberOfColumns?: number
    ) => {
      if (maxNumberOfColumns != undefined) {
        setHallSeats((prevSeats) => {
          // Seat not exist or exisst, seatType is undefined
          if (seatType === undefined) {
            return prevSeats.filter((seat) => seat.seatNr !== seatNumber);
          }

          // Seat exist, seatType is defined
          const seatExists = prevSeats.some((seat) => seat.seatNr === seatNumber);
          if (seatExists) {
            return prevSeats.map((seat) => {
              if (seat.seatNr === seatNumber) {
                return {
                  ...seat,
                  seatType: seatType,
                };
              }
              return seat;
            });
          }

          // Seat not exist, seatType is defined
          return [
            ...prevSeats,
            {
              seatNr: seatNumber,
              row: Math.ceil(seatNumber / maxNumberOfColumns),
              gridRow: Math.ceil(seatNumber / maxNumberOfColumns),
              column: ((seatNumber - 1) % maxNumberOfColumns) + 1,
              isAvailable: true,
              gridColumn: ((seatNumber - 1) % maxNumberOfColumns) + 1,
              seatType: seatType,
            } as Seat,
          ];
        });
      }
    };

    const generateContextMenuItems = () => {
      if (!seatTypes) return [];

      const items = seatTypes.map((seatType) => ({
        label: seatType.name,
        action: (selectedSeatNr?: number) => {
          if (selectedSeatNr !== undefined)
            changeSeatType(selectedSeatNr, seatType, currentMaxNumberCols);
        },
      }));

      items.push({
        label: "Miejsce nieaktywne",
        action: (selectedSeatNr?: number) => {
          if (selectedSeatNr !== undefined)
            changeSeatType(selectedSeatNr, undefined, currentMaxNumberRows);
        },
      });

      return items;
    };

    const getSeats = (maxNumberOfSeatsRows?: number, maxNumberOfSeatsColumns?: number) => {
      if (maxNumberOfSeatsRows !== undefined && maxNumberOfSeatsColumns !== undefined) {
        return (
          <div className="flex flex-col gap-[10px]" onContextMenu={(e) => e.preventDefault()}>
            {Array.from({ length: maxNumberOfSeatsRows }, (_, rowIndex) => (
              <div
                key={rowIndex}
                className="grid gap-[10px]"
                style={{
                  gridTemplateColumns: `repeat(${maxNumberOfSeatsColumns}, minmax(0, 1fr))`,
                }}
              >
                {Array.from({ length: maxNumberOfSeatsColumns }, (_, colIndex) => {
                  const seatNumber = rowIndex * maxNumberOfSeatsColumns + colIndex + 1;
                  const seat = hallSeats.find((s) => s.seatNr == seatNumber);
                  //const isAvailable = seatNumbers.includes(seatNumber);

                  return (
                    <div
                      key={`${rowIndex}-${colIndex}`}
                      className={`border px-[8px] py-[4px] text-center text-[12px] flex flex-col justify-center gap-[2px] items-center ${
                        seat
                          ? "border-gray-300 text-white hover:cursor-pointer"
                          : "border-gray-400 bg-gray-200 text-gray-500"
                      }`}
                      // onContextMenu={(event) => onRightClick(event, seatNumber)}
                      onContextMenu={(e) =>
                        handleContextMenu(
                          e,
                          seatNumber,
                          maxNumberOfSeatsRows,
                          maxNumberOfSeatsColumns
                        )
                      }
                      style={{ backgroundColor: seat?.seatType?.seatColor }}
                    >
                      <FontAwesomeIcon icon={faCouch} fontSize={12} />
                      <p className="hover:cursor-default text-[11px]">{seatNumber}</p>
                    </div>
                  );
                })}
              </div>
            ))}
          </div>
        );
      }
    };

    const handleContextMenu = (
      event: React.MouseEvent,
      seatNumber: number,
      _rows: number,
      cols: number
    ) => {
      event.preventDefault();
      //setSelectedSeatNr(seatNumber);
      const x = 70 + ((seatNumber - 1) % cols) * 48;
      const y = 140 + Math.floor((seatNumber - 1) / cols) * 45;
      setContextMenu({
        selectedSeatNr: seatNumber,
        position: { x: x, y: y },
        items: generateContextMenuItems(),
      });
    };

    const handleCloseContextMenu = () => {
      setContextMenu(null);
    };

    const seatAsRequest = (seat: Seat, maxNumberOfColumns?: number): SeatRequest | undefined => {
      console.log(maxNumberOfColumns);
      if (maxNumberOfColumns != undefined) {
        const seatRequest = {
          seatNr: seat.seatNr,
          row: Math.ceil(seat.seatNr / maxNumberOfColumns),
          gridRow: Math.ceil(seat.seatNr / maxNumberOfColumns),
          column: ((seat.seatNr - 1) % maxNumberOfColumns) + 1,
          gridColumn: ((seat.seatNr - 1) % maxNumberOfColumns) + 1,
          seatTypeId: seat.seatType?.id,
        } as SeatRequest;

        const parsedSeatRequest = SeatSchema.safeParse(seatRequest);

        if (!parsedSeatRequest.success) {
          const errors = parsedSeatRequest.error.errors
            .map((err) => `${err.path.join(".")}: ${err.message}`)
            .join(", ");
          throw new Error(errors);
        }

        return parsedSeatRequest.data;
      }
      return undefined;
    };

    const onSubmit: SubmitHandler<HallRequest> = async (data) => {
      console.log(data);
      const requestSeats: SeatRequest[] = [];
      hallSeats.forEach((seat) => {
        try {
          const validatedSeat = seatAsRequest(seat, currentMaxNumberCols);
          if (validatedSeat == undefined) {
            throw Error("Wystąpił błąd podczas walidacji miejsc");
          }
          requestSeats.push(validatedSeat);
        } catch (error) {
          const errorMessage = error instanceof Error ? error.message : "Nieznany błąd.";
          toast.error(errorMessage);
          return;
        }
      });

      const requestHall = {
        hallNr: data.hallNr,
        rentalPricePerHour: data.rentalPricePerHour,
        floor: data.floor,
        hallTypeId: data.hallTypeId,
        totalLength: data.totalLength,
        totalWidth: data.totalWidth,
        stageLength: data.stageLength,
        stageWidth: data.stageWidth,
        maxNumberOfSeatsRows: data.maxNumberOfSeatsRows,
        maxNumberOfSeatsColumns: data.maxNumberOfSeatsColumns,
        seats: requestSeats,
      } as HallRequest;

      console.log(requestSeats);
      setPromisePending(true);
      await toast.promise(postItem({ body: requestHall }), {
        pending: "Wykonywanie żądania",
        success: "Sala została utworzona pomyślnie",
        error: "Wystąpił błąd podczas tworzenia nowej sali",
      });
      setPromisePending(false);
      setActionPerformed(true);
    };

    useEffect(() => {
      if (actionPerformed) {
        if (statusCode == HTTPStatusCode.Created) {
          onDialogSuccess();
          reset();
        }
        setActionPerformed(false);
      }
    }, [actionPerformed]);

    return (
      <div>
        <Dialog ref={ref} minHeight={600} maxHeight={1200} marginTop={20}>
          <FormProvider {...methods}>
            <form
              className="flex flex-col justify-center items-center gap-6"
              onSubmit={handleSubmit(onSubmit)}
            >
              <div className="flex flex-row justify-center items-start gap-3">
                <div className="flex flex-col justify-center items-center px-5 pb-2 gap-6">
                  <div className="flex flex-col justify-center items-center gap-2">
                    <h2>Tworzenie sali</h2>
                  </div>
                  <div className="flex flex-col justify-center items-center gap-3">
                    {/* <div>stage</div> */}
                    <div className="flex flex-col gap-2">
                      {getSeats(currentMaxNumberRows, currentMaxNumberCols)}
                      {contextMenu && (
                        <ContextMenu
                          selectedSeatNr={contextMenu.selectedSeatNr}
                          items={contextMenu.items}
                          position={contextMenu.position}
                          onClose={handleCloseContextMenu}
                        />
                      )}
                    </div>
                  </div>
                </div>
                <div className="self-stretch min-w-[1px] bg-black"></div>
                <div className="pl-3 flex flex-col justify-centers items-start gap-4">
                  <article className="flex flex-col justify-start items-start gap-1 w-full">
                    <h5 className="text-center font-semibold text-base w-full">Typy miejsc:</h5>
                    <div className="flex justify-start flex-col gap-2">
                      {seatTypes.map((type, index) => (
                        <div key={index} className="flex flex-row justify-start items-center gap-2">
                          <div
                            className="py-[2px] px-[6px]"
                            style={{ backgroundColor: type.seatColor }}
                          >
                            <FontAwesomeIcon
                              fontSize={12}
                              icon={faCouch}
                              color="white"
                            ></FontAwesomeIcon>
                          </div>
                          <p className="text-[14px]">{type.name}</p>
                        </div>
                      ))}
                      <div className="flex flex-row justify-start items-center gap-2">
                        <div className="py-[2px] px-[6px] border-gray-400 bg-gray-200">
                          <FontAwesomeIcon
                            fontSize={12}
                            icon={faCouch}
                            color="#6b7280"
                          ></FontAwesomeIcon>
                        </div>
                        <p className="text-[14px]">Miejsce nieaktywne</p>
                      </div>
                    </div>
                  </article>
                  <div className="flex flex-row justify-center items-center gap-6 w-full">
                    <div className="flex flex-col justify-center items-center gap-2 min-w-[220px]">
                      <Input
                        label="Nr sali"
                        type="number"
                        name="hallNr"
                        min={1}
                        error={errors.hallNr}
                        errorHeight={15}
                      />
                      <Input
                        label="Cena za 1h (zł)"
                        type="number"
                        step={0.01}
                        name="rentalPricePerHour"
                        min={0}
                        max={999}
                        error={errors.rentalPricePerHour}
                        errorHeight={15}
                      />
                      <Input
                        label="Piętro"
                        type="number"
                        name="floor"
                        min={1}
                        max={4}
                        error={errors.floor}
                        errorHeight={15}
                      />
                      <Select
                        label="Typ sali"
                        name="hallTypeId"
                        selectedOption={selectedOption}
                        optionValues={selectOptions}
                        error={errors.hallTypeId}
                        errorHeight={15}
                      />
                      <Input
                        label="Długość sali (m)"
                        type="number"
                        name="totalLength"
                        step={0.01}
                        min={4}
                        max={99.99}
                        error={errors.totalLength}
                        errorHeight={15}
                      />
                    </div>
                    <div className="flex flex-col justify-center items-center gap-2 min-w-[220px]">
                      <Input
                        label="Szerokość sali (m)"
                        type="number"
                        name="totalWidth"
                        step={0.01}
                        min={4}
                        max={99.99}
                        error={errors.totalWidth}
                        errorHeight={15}
                      />
                      <Input
                        label="Długość sceny (m)"
                        type="number"
                        name="stageLength"
                        step={0.01}
                        min={3}
                        max={30}
                        error={errors.stageLength}
                        errorHeight={15}
                      />
                      <Input
                        label="Szerokość sceny (m)"
                        type="number"
                        name="stageWidth"
                        step={0.01}
                        min={3}
                        max={30}
                        error={errors.stageWidth}
                        errorHeight={15}
                      />
                      <Input
                        label="Max ilość rzędów"
                        type="number"
                        name="maxNumberOfSeatsRows"
                        min={1}
                        max={20}
                        error={errors.maxNumberOfSeatsRows}
                        errorHeight={15}
                      />
                      <Input
                        label="Max ilość kolumn"
                        type="number"
                        name="maxNumberOfSeatsColumns"
                        min={1}
                        max={20}
                        error={errors.maxNumberOfSeatsColumns}
                        errorHeight={15}
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div className="flex flex-row justify-center items-center gap-2">
                <Button
                  text="Anuluj"
                  width={145}
                  height={45}
                  icon={faXmark}
                  iconSize={18}
                  style={ButtonStyle.DefaultGray}
                  action={onDialogClose}
                ></Button>
                <FormButton
                  text={"Zatwierdź"}
                  rounded={999}
                  width={145}
                  height={45}
                  icon={faCheck}
                  iconSize={18}
                  isSubmitting={isSubmitting}
                />
              </div>
            </form>
          </FormProvider>
        </Dialog>
      </div>
    );
  }
);
export default CreateHallDialog;
