namespace Frank.WireFishApp.FileWriting;

public record FileWriteRequest(FileInfo FileInfo, string Content, bool Append = true);