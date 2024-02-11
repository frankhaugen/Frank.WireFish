namespace Frank.WireFish.Models;

public record FileWriteRequest(FileInfo FileInfo, string Content, bool Append = true);