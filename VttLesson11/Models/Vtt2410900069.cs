using System;
using System.Collections.Generic;

namespace VttLesson11.Models;

public partial class Vtt2410900069
{
    public long Id { get; set; }

    public string VttName { get; set; } = null!;

    public string? VttGender { get; set; }

    public DateOnly? VttBirthDay { get; set; }

    public string? VttEmail { get; set; }

    public string? VttPhone { get; set; }

    public bool VttActive { get; set; }
}
