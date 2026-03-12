/**
 * @Author: Carlos Galeano
 * @Date:   2026-03-11 16:53:47
 * @Last Modified by:   Carlos Galeano
 * @Last Modified time: 2026-03-11 16:53:51
 */
public record UserRegisterDto(string Username, string Password);
public record UserLoginDto(string Username, string Password);

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password_Hash { get; set; } = "";
}