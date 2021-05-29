import StringsBase from "../Strings/StringsBase";

export default interface IPotterRepository<TStrings extends StringsBase> {
    strings: TStrings;
    busy: boolean;
}
