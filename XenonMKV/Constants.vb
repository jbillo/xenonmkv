Module Constants

    Public Const BUILD_DATE As String = "20110301"
    Public Const BUILD_REVISION As String = "39"
    Public Const BUILD_VERSION As String = BUILD_DATE & "_" & BUILD_REVISION
    Public Const BUILD_CODENAME As String = "war59312"

    Public Const NERO_AAC_URL As String = "http://ftp6.nero.com/tools/NeroAACCodec-1.5.1.zip"
    Public Const HELP_WEBSITE As String = "http://xenonmkv.ev98.net"
    Public Const HELP_DOWNLOAD As String = HELP_WEBSITE & "/download/"
    Public Const HELP_FORUM As String = HELP_WEBSITE & "/forum/"
    Public Const HELP_UPDATE As String = HELP_WEBSITE & "/latest.txt"
    Public Const CREDIT_ICONS As String = "http://famfamfam.com/lab/icons/silk/"

    Public Const MP4BOX_DEFAULT_PATH As String = "\tools\mp4box\MP4Box_default.exe"
    Public Const MP4BOX_APR13_PATH As String = "\tools\mp4box\MP4Box_20080413.exe"
    Public Const MP4BOX_044_PATH As String = "\tools\mp4box\MP4Box_044.exe"

    Public Const DEFAULT_LANGUAGE As String = "english.xml"
    Public Const DEFAULT_FPS As Double = 23.976
    Public Const DEFAULT_PAR As String = "64:45"

    Public Const REGEDIT_MP4_ENABLE_BASE As String = "v7-WMP11-MP4-M2TS-M4V-M4A"

    Public AppPath As String = Application.StartupPath

    Public Const TASK_PARSEMKV = 1
    Public Const TASK_EXTRACTMKV = 2
    Public Const TASK_HEXEDIT = 3
    Public Const TASK_AUDIODUMP = 4
    Public Const TASK_MPLAYER = 5
    Public Const TASK_NEROAAC = 6
    Public Const TASK_MP4BOX = 7

    Public Const TASK_MPLAYER_FALLBACK1 = 8
    Public Const TASK_AZID = 9
    Public Const TASK_FFMPEG = 10
    Public Const TASK_VALDEC = 11

    Public Const TASK_SPLIT = -1
    Public Const TASK_GETMKV = -2
    Public Const TASK_ERROR = -999
    Public Const TASK_ABORT = -998

    Public Const OUTPUT_STRING = 0
    Public Const OUTPUT_PROGRESS = 1

    Public Const DEFAULT_SPLIT_MB As Integer = 4096
    Public Const DEFAULT_OUTPUT_EXTENSION As String = "mp4"

    Public Const UI_ADDLOG = 0

    ' Images for log: accept | error | hourglass | information | stop
    Public Const IMG_ACCEPT = 0
    Public Const IMG_ERROR = 1
    Public Const IMG_WAIT = 2
    Public Const IMG_INFO = 3
    Public Const IMG_STOP = 4
End Module
